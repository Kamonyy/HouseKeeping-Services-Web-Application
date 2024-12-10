using HousekeepingAPI.Data;
using HousekeepingAPI.Interfaces;
using HousekeepingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HousekeepingAPI.Repository
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly ApplicationDbContext _context;

        public ServiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Models.Service>> GetAllAsync()
        {
            return await _context.Services
                .Include(s => s.ServiceSubCategory)
                .ThenInclude(sc => sc.SubCategory)
                .ToListAsync();
        }

        public async Task<Models.Service?> GetByIdAsync(int id)
        {
            return await _context.Services
                .Include(s => s.ServiceSubCategory)
                .ThenInclude(sc => sc.SubCategory)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<ServiceSubCategory>> CreateAsync(Models.Service service, List<int> subCategoryIds)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.Services.AddAsync(service);
                await _context.SaveChangesAsync();

                var serviceSubCategories = subCategoryIds.Select(subCategoryId => new ServiceSubCategory
                {
                    ServiceId = service.Id,
                    SubCategoryId = subCategoryId,
                }).ToList();

                await _context.ServiceSubCategories.AddRangeAsync(serviceSubCategories);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return serviceSubCategories;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

        }
    }
}
