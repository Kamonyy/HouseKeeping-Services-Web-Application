using HousekeepingAPI.Data;
using HousekeepingAPI.Interfaces;
using HousekeepingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HousekeepingAPI.Repository
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public SubCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ICollection<SubCategory>>? GetAllAsync()
        {
            return await _context.SubCategories
                .Include(sc => sc.Category)
                .ToListAsync();
        }

        public async Task<ICollection<SubCategory>> GetAllByCategoryAsync(int categoryId)
        {
            return await _context.SubCategories
                .Where(sc => sc.CategoryId == categoryId)
                .Include(sc => sc.Category)
                .ToListAsync();
        }
        public async Task<SubCategory?> GetByIdAsync(int id)
        {
            return await _context.SubCategories
                .Include(sc => sc.Category)
                .FirstOrDefaultAsync(sc => sc.id == id);
        }

        public async Task<ICollection<SubCategory>> GetAllByIdsAsync(IEnumerable<int> ids)
        {
            return await _context.SubCategories
                .Where(sc => ids.Contains(sc.id))
                .ToListAsync();
        }

        public async Task<SubCategory?> CreateAsync(SubCategory subCategory)
        {
            await _context.SubCategories.AddAsync(subCategory);
            await _context.SaveChangesAsync();
            return subCategory;
        }
        public async Task<SubCategory?> UpdateAsync(int id, SubCategory subCategory)
        {
            var existingSubCategory = await _context.SubCategories.FirstOrDefaultAsync(sc => sc.id == id);

            if (existingSubCategory == null)
                return null;

            existingSubCategory.Name = subCategory.Name;
            existingSubCategory.CategoryId = subCategory.CategoryId;

            await _context.SaveChangesAsync();

            return existingSubCategory;
        }

        public async Task<SubCategory?> DeleteAsync(int id)
        {
            var subCategory = await _context.SubCategories.FirstOrDefaultAsync(sc => sc.id == id);

            if (subCategory == null)
                return null;

            _context.SubCategories.Remove(subCategory);
            await _context.SaveChangesAsync();
            return subCategory;
        }

        
    }
}
