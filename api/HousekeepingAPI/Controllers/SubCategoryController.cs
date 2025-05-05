using AutoMapper;
using HousekeepingAPI.Dto.SubCategory;
using HousekeepingAPI.Interfaces;
using HousekeepingAPI.Models;
using HousekeepingAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace HousekeepingAPI.Controllers
{
    [Route("api/subCategory")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private readonly ISubCategoryRepository _subCategoryRepo;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _CategoryRepo;

        public SubCategoryController(ISubCategoryRepository subCategoryRepository, IMapper mapper, ICategoryRepository CategoryRepository)
        {
            _subCategoryRepo = subCategoryRepository;
            _mapper = mapper;
            _CategoryRepo = CategoryRepository;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var subCategories = _mapper.Map<List<SubCategoryDto>>(await _subCategoryRepo.GetAllAsync());

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(subCategories);
        }

        [HttpGet("byCategory/{categoryId}")]
        public async Task<IActionResult> GetAllByCategory(int categoryId)
        {
            var subCategories = _mapper.Map<List<SubCategoryDto>>(await _subCategoryRepo.GetAllByCategoryAsync(categoryId));

            if (subCategories.Count == 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(subCategories);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubCategoryById(int id)
        {
            var subCategory = _mapper.Map<SubCategoryDto>(await _subCategoryRepo.GetByIdAsync(id));

            if (subCategory == null)
                return NotFound();

            return Ok(subCategory);
        }
        [HttpPost]
        public async Task<IActionResult> CreateSubCategory([FromBody] CreateSubCategoryDto createDto)
        {
            if (createDto == null || string.IsNullOrEmpty(createDto.Name))
                return BadRequest("Invalid data.");
            var category = await _CategoryRepo.GetByIdAsync(createDto.CategoryId);

            if (category == null)
                return NotFound("Category not found");

            var subCategory = new SubCategory
            {
                Name = createDto.Name,
                CategoryId = createDto.CategoryId,
                Category = category,
            };

            var createdSubCategory = await _subCategoryRepo.CreateAsync(subCategory);

            if (createdSubCategory == null)
                return StatusCode(500, "Unable to save SubCategory.");

            return CreatedAtAction(nameof(GetSubCategoryById), new { id = createdSubCategory.Id }, createdSubCategory);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubCategory(int id)
        {
            var deletedSubCategory = await _subCategoryRepo.DeleteAsync(id);

            if (deletedSubCategory == null)
            {
                return NotFound($"SubCategory with ID {id} was not found.");
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubCategory(int id, [FromBody] UpdateSubCategoryDto updateDto)
        {
            if (updateDto == null || string.IsNullOrEmpty(updateDto.Name))
                return BadRequest("Invalid data provided.");

            var existingSubCategory = await _subCategoryRepo.GetByIdAsync(id);
            if (existingSubCategory == null)
                return NotFound($"SubCategory with ID {id} not found.");
            
            var existingCategory = await _CategoryRepo.GetByIdAsync(updateDto.CategoryId);
            if(existingCategory == null)
                return NotFound($"Category not Found");

            existingSubCategory.Name = updateDto.Name;
            existingSubCategory.CategoryId = updateDto.CategoryId;

            var updatedSubCategory = await _subCategoryRepo.UpdateAsync(id, existingSubCategory);

            if (updatedSubCategory == null)
                return StatusCode(500, "Unable to update the SubCategory.");

            return Ok(updatedSubCategory);
        }
    }
}
