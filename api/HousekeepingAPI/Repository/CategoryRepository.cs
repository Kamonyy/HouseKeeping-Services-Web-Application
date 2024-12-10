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
    }
}
