using HousekeepingAPI.Interfaces;
using HousekeepingAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HousekeepingAPI.Controllers
{
        [Route("api/category")]
        [ApiController]
    public class CategoryController : Controller
    {

        private ICategoryRepository _CategoryRepository;

        public CategoryController(ICategoryRepository CategoryRepository)
        {
            _CategoryRepository = CategoryRepository;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetAll() 
        {
            var categories = _CategoryRepository.GetAll();

            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(categories);
        }

        [HttpGet("{CategoryId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        public IActionResult GetCategory(int CategoryId)
        {
            var categories = _CategoryRepository.GetById(CategoryId);

            if (categories == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(categories);
        }

    }
}
