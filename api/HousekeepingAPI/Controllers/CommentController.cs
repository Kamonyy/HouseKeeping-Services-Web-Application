using Microsoft.AspNetCore.Mvc;
using HousekeepingAPI.Interfaces;
using HousekeepingAPI.Models;
using HousekeepingAPI.Dto.Comment;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HousekeepingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IServiceRepository _serviceRepository;

        public CommentController(ICommentRepository commentRepository, IServiceRepository serviceRepository)
        {
            _commentRepository = commentRepository;
            _serviceRepository = serviceRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetComment(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }

        [HttpGet("service/{serviceId}")]
        public async Task<IActionResult> GetCommentsByServiceId(int serviceId)
        {
            var comments = await _commentRepository.GetAllByServiceIdAsync(serviceId);
            return Ok(comments);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CommentCreateDto commentDto)
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

            var service = await _serviceRepository.GetServiceById(commentDto.ServiceId);
            if (service == null) return BadRequest("Service does not exist.");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var createdComment = await _commentRepository.CreateAsync(commentDto, userId);
            return CreatedAtAction(nameof(GetComment), new { id = createdComment.Id }, createdComment);
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
