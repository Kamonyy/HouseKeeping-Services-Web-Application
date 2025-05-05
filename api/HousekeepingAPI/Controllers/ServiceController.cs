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
        
        [HttpGet("provider")]
        [Authorize(Roles = "Provider")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ServiceListDto>))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetProviderServices()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var services = await _serviceRepository.GetByProviderIdAsync(userId);
                return Ok(services);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting provider services");
                return StatusCode(500, "Internal server error");
            }
        }
        
        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ServiceListDto>))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllServicesForAdmin()
        {
            try
            {
                var services = await _serviceRepository.GetAllForAdminAsync();
                return Ok(services);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all services for admin");
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
        [Authorize(Roles = "Provider")]
        [ProducesResponseType(201, Type = typeof(ServiceDetailsDto))]
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
                
                // Map the service to a DTO to avoid circular references
                var serviceDetailsDto = _mapper.Map<ServiceDetailsDto>(service);
                
                return CreatedAtAction(nameof(GetService), new { id = service.Id }, serviceDetailsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating service");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Provider")]
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
                
                var service = await _serviceRepository.GetServiceById(id);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (service.UserId != userId)
                    return Forbid();
                    
                // Reset approval status when service is updated
                updateServiceDto.IsApproved = false;

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
        
        [HttpPut("approve/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ApproveService(int id)
        {
            try
            {
                if (!await _serviceRepository.Exists(id))
                    return NotFound();

                var success = await _serviceRepository.ApproveServiceAsync(id);
                if (!success)
                    return StatusCode(500, "Could not approve service");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error approving service with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }
        
        [HttpPut("revoke/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RevokeServiceApproval(int id)
        {
            try
            {
                if (!await _serviceRepository.Exists(id))
                    return NotFound();

                var success = await _serviceRepository.RevokeServiceApprovalAsync(id);
                if (!success)
                    return StatusCode(500, "Could not revoke service approval");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error revoking approval for service with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Provider,Admin")]
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
                var isAdmin = User.IsInRole("Admin");

                // Allow admins to delete any service, but providers can only delete their own
                if (!isAdmin && service.UserId != userId)
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
