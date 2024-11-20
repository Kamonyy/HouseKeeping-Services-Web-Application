using AutoMapper;
using HousekeepingAPI.Dto;
using HousekeepingAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HousekeepingAPI.Controllers
{

    [Route("api/service")]
    [ApiController]
    public class ServiceController : Controller
    {

        private IServiceRepository _ServiceRepository;
        private readonly IMapper _mapper;

        public ServiceController(IServiceRepository ServiceRepository, IMapper mapper)
        {
            _ServiceRepository = ServiceRepository;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Models.Service>))]
        public IActionResult GetAll()
        {
            var services = _mapper.Map<List<ServiceDto>>(_ServiceRepository.GetAll());

            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(services);
        }

        [HttpGet("{ServiceId}")]
        [ProducesResponseType(200, Type = typeof(Models.Service))]
        public IActionResult GetService(int ServiceId)
        {
            var services = _mapper.Map<List<ServiceDto>>(_ServiceRepository.GetById(ServiceId));

            if (services == null)
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(services);
        }
    }
}
