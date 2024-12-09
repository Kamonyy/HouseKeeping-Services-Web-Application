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
        public ICollection<SubCategory> GetAll()
        {
            return _context.SubCategories
                .Include(sc => sc.Category)
                .ToList();
        }

        public ICollection<SubCategory> GetAllByCategoty(int CategoryId)
        {
            return _context.SubCategories
                .Where(sc =>  sc.CategoryId == CategoryId)
                .Include(sc => sc.Category)
                .ToList();
        }
    }
}
