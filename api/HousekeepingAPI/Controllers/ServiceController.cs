using AutoMapper;
using HousekeepingAPI.Dto.Service;
using HousekeepingAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HousekeepingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceController> _logger;

        public ServiceController(
            IServiceRepository serviceRepository,
            IMapper mapper,
            ILogger<ServiceController> logger)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ServiceListDto>))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllServices()
        {
            try
            {
                var services = await _serviceRepository.GetAllAsync();
                return Ok(services);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all services");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ServiceDetailsDto))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetService(int id)
        {
            try
            {
                if (!await _serviceRepository.Exists(id))
                    return NotFound();

                var service = await _serviceRepository.GetByIdAsync(id);
                return Ok(service);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting service with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("bySubCategory/{subCategoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ServiceListDto>))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetServicesBySubCategory(int subCategoryId)
        {
            try
            {
                var services = await _serviceRepository.GetBySubCategoryIdAsync(subCategoryId);
                return Ok(services);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting services for subcategory ID {subCategoryId}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(201, Type = typeof(Models.Service))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateService([FromBody] CreateServiceDto serviceDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var service = await _serviceRepository.CreateAsync(serviceDto, userId);
                return CreatedAtAction(nameof(GetService), new { id = service.Id }, service);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating service");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateService(int id, [FromBody] UpdateServiceDto updateServiceDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!await _serviceRepository.Exists(id))
                    return NotFound();

                var success = await _serviceRepository.UpdateAsync(id, updateServiceDto);
                if (!success)
                    return StatusCode(500, "Could not update service");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating service with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteService(int id)
        {
            try
            {
                if (!await _serviceRepository.Exists(id))
                    return NotFound();

                var service = await _serviceRepository.GetServiceById(id);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


                if (service.UserId != userId)
                    return Forbid();

                var success = await _serviceRepository.DeleteAsync(id);
                if (!success)
                    return StatusCode(500, "Could not delete service");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting service with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
