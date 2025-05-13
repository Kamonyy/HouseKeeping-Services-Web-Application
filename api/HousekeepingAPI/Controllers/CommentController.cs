using Microsoft.AspNetCore.Mvc;
using HousekeepingAPI.Interfaces;
using HousekeepingAPI.Models;
using HousekeepingAPI.Dto.Comment;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using AutoMapper;

namespace HousekeepingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public CommentController(ICommentRepository commentRepository, IServiceRepository serviceRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetComment(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            var dto = _mapper.Map<CommentDto>(comment);
            return Ok(dto);
        }

        [HttpGet("service/{serviceId}")]
        public async Task<IActionResult> GetCommentsByServiceId(int serviceId)
        {
            var comments = await _commentRepository.GetAllByServiceIdAsync(serviceId);
            var dtos = comments.Select(c => _mapper.Map<CommentDto>(c)).ToList();
            return Ok(dtos);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CommentCreateDto commentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (commentDto.ServiceId <= 0)
                {
                    return BadRequest("A valid ServiceId must be provided.");
                }

                if (string.IsNullOrWhiteSpace(commentDto.Content))
                {
                    return BadRequest("Comment content must not be empty.");
                }
                
                // Validate the rating is between 0 and 5
                if (commentDto.Rating < 0 || commentDto.Rating > 5)
                {
                    return BadRequest("Rating must be between 0 and 5.");
                }

                var service = await _serviceRepository.GetServiceById(commentDto.ServiceId);
                if (service == null) return BadRequest("Service does not exist.");

                // Get the current user ID from claims
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User ID could not be determined. Please log in again.");
                }

                // Create and return the comment
                var createdComment = await _commentRepository.CreateAsync(commentDto, userId);
                return CreatedAtAction(nameof(GetComment), new { id = createdComment.Id }, _mapper.Map<CommentDto>(createdComment));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] CommentUpdateDto commentDto)
        {
            if (id != commentDto.Id)
            {
                return BadRequest("Comment ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (commentDto.ServiceId <= 0)
            {
                return BadRequest("A valid ServiceId must be provided.");
            }

            if (string.IsNullOrWhiteSpace(commentDto.Content))
            {
                return BadRequest("Comment content must not be empty.");
            }
            
            // Validate the rating is between 0 and 5
            if (commentDto.Rating < 0 || commentDto.Rating > 5)
            {
                return BadRequest("Rating must be between 0 and 5.");
            }

            var service = await _serviceRepository.GetServiceById(commentDto.ServiceId);
            if (service == null) return BadRequest("Service does not exist.");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var existingComment = await _commentRepository.GetByIdAsync(id);
            if (existingComment == null)
                return NotFound();

            if (existingComment.UserId != userId)
                return Forbid();

            commentDto.UserId = userId;

            var updated = await _commentRepository.UpdateAsync(commentDto);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            // Get the current authenticated user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Verify the comment belongs to the current user
            var existingComment = await _commentRepository.GetByIdAsync(id);
            if (existingComment == null)
                return NotFound();

            if (existingComment.UserId != userId)
                return Forbid();

            var deleted = await _commentRepository.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
