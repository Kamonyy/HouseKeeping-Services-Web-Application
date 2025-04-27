using AutoMapper;
using HousekeepingAPI.Data;
using HousekeepingAPI.Dto.Service;
using HousekeepingAPI.Interfaces;
using HousekeepingAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage; // Add this using directive

namespace HousekeepingAPI.Repository
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ServiceRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceDetailsDto> GetByIdAsync(int id)
        {
            var service = await _context.Services
                .Include(s => s.Provider)
                .Include(s => s.ServiceSubCategory)
                    .ThenInclude(ssc => ssc.SubCategory)
                .FirstOrDefaultAsync(s => s.Id == id);

            return _mapper.Map<ServiceDetailsDto>(service);
        }

        public async Task<ICollection<ServiceListDto>> GetAllAsync()
        {
            var services = await _context.Services
                .Include(s => s.Provider)
                .Include(s => s.ServiceSubCategory)
                    .ThenInclude(ssc => ssc.SubCategory)
                .ToListAsync();

            return _mapper.Map<List<ServiceListDto>>(services);
        }

        public async Task<Models.Service> CreateAsync(CreateServiceDto serviceDto, string userId)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = await _context.Users.FindAsync(userId);

                if (user == null)
                {
                    throw new Exception("User not found");
                }
                var service = _mapper.Map<Models.Service>(serviceDto);
                service.UserId = userId;
                service.Provider = user;
                service.CreatedDate = DateTime.UtcNow;

                await _context.Services.AddAsync(service);
                await _context.SaveChangesAsync();

                var serviceSubCategories = serviceDto.SubCategoryIds.Select(id => new ServiceSubCategory
                {
                    ServiceId = service.Id,
                    SubCategoryId = id
                }).ToList();

                await _context.ServiceSubCategories.AddRangeAsync(serviceSubCategories);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return service;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
            finally
            {
                if (transaction != null)
                {
                    await transaction.DisposeAsync();
                }
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdateServiceDto serviceDto)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return false;

            _mapper.Map(serviceDto, service);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return false;

            _context.Services.Remove(service);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.Services.AnyAsync(s => s.Id == id);
        }

        public async Task<Models.Service> GetServiceById(int id)
        {
            return await _context.Services.FindAsync(id);
        }
    }
}
