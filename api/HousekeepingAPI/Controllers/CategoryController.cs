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
        public async Task<IActionResult> GetAll() 
        {
            var categories = await _CategoryRepository.GetAllAsync();

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(categories);
        }

        [HttpGet("{CategoryId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        public async Task<IActionResult> GetCategory(int CategoryId)
        {
            var categories = await _CategoryRepository.GetByIdAsync(CategoryId);

            if (categories == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(categories);
        }

    }
}
