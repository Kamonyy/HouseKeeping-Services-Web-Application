using AutoMapper;
using HousekeepingAPI.Dto.Service;
using HousekeepingAPI.Extentions;
using HousekeepingAPI.Interfaces;
using HousekeepingAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HousekeepingAPI.Controllers
{
    [Route("api/service")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManger;
        private readonly ISubCategoryRepository _subCategoryRepo;

        public ServiceController(IServiceRepository serviceRepository, IMapper mapper,
            UserManager<AppUser> userManger    
            , ISubCategoryRepository subCategoryRepository)
        {
            _serviceRepo = serviceRepository;
            _mapper = mapper;
            _userManger = userManger;
            _subCategoryRepo = subCategoryRepository;

        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ServiceDto>))]
        public async Task<IActionResult> GetAll()
        {
            var services = await _serviceRepo.GetAllAsync();
            var mappedServices = _mapper.Map<List<ServiceDto>>(services);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(mappedServices);
        }

        [HttpGet("{serviceId}")]
        [ProducesResponseType(200, Type = typeof(ServiceDto))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetServiceById(int serviceId)
        {
            var service = await _serviceRepo.GetByIdAsync(serviceId);

            if (service == null)
                return NotFound();

            var mappedService = _mapper.Map<ServiceDto>(service);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(mappedService);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(201, Type = typeof(ServiceDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Create([FromBody] CreateServiceDto createServiceDto)
        {
            if (createServiceDto == null || string.IsNullOrEmpty(createServiceDto.Title))
                return BadRequest("Invalid data.");

            if (createServiceDto.SubCategoryIds == null || !createServiceDto.SubCategoryIds.Any())
                return BadRequest("SubCategoryIds cannot be null or empty.");

            var subCategories = await _subCategoryRepo.GetAllByIdsAsync(createServiceDto.SubCategoryIds);

            if (subCategories.Count != createServiceDto.SubCategoryIds.Count)
                return NotFound("One or more Sub-Categories do not exist.");

            var service = _mapper.Map<Models.Service>(createServiceDto);

            service.Username = User.GetUsername();
            var createdServiceSubCategories = await _serviceRepo.CreateAsync(service, createServiceDto.SubCategoryIds);

            if (createdServiceSubCategories == null || !createdServiceSubCategories.Any())
                return StatusCode(500, "Unable to save Service or its relationships.");

            var mappedResponse = _mapper.Map<ServiceDto>(service);

            return CreatedAtAction(nameof(GetServiceById), new { serviceId = service.Id }, mappedResponse);
        }

    }
}
