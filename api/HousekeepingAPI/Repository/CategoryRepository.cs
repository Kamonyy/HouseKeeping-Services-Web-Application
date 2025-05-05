using HousekeepingAPI.Data;
using HousekeepingAPI.Interfaces;
using HousekeepingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HousekeepingAPI.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Category>> GetAllAsync()
        {
            return await _context.Categories.OrderBy(c => c.Id).ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<Category?> CreateAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> UpdateAsync(int id, Category category)
        {
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (existingCategory == null)
                return null;

            existingCategory.Name = category.Name;

            await _context.SaveChangesAsync();

            return existingCategory;
        }

        public async Task<Category?> DeleteAsync(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return null;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return category;
        }
    }
}
