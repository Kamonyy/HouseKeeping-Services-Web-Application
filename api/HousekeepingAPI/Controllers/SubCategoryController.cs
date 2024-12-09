using AutoMapper;
using HousekeepingAPI.Dto.SubCategory;
using HousekeepingAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HousekeepingAPI.Controllers
{
    [Route("api/subCategory")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private ISubCategoryRepository _subCategoryRepository;
        private readonly IMapper _mapper;

        public SubCategoryController(ISubCategoryRepository SubCategoryRepository, IMapper mapper)
        {
            _subCategoryRepository = SubCategoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var subCategories = _mapper.Map<List<SubCategoryDto>>(_subCategoryRepository.GetAll());

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(subCategories);
        }

        [HttpGet("{categoryId}")]
        public IActionResult GetAllByCategory(int categoryId)
        {
            var subCategories = _mapper.Map<List<SubCategoryDto>>(_subCategoryRepository.GetAllByCategoty(categoryId));

            if (subCategories.Count == 0)
                return NotFound();

            if(!ModelState.IsValid) 
                return BadRequest();

            return Ok(subCategories);
        }
    }
}
