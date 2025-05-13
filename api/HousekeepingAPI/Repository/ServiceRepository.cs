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
                        .ThenInclude(sc => sc.Category)
                .FirstOrDefaultAsync(s => s.Id == id);

            return _mapper.Map<ServiceDetailsDto>(service);
        }

        public async Task<ICollection<ServiceListDto>> GetAllAsync()
        {
            var services = await _context.Services
                .Include(s => s.Provider)
                .Include(s => s.ServiceSubCategory)
                    .ThenInclude(ssc => ssc.SubCategory)
                        .ThenInclude(sc => sc.Category)
                .Where(s => s.IsApproved) // Only return approved services
                .ToListAsync();

            return _mapper.Map<List<ServiceListDto>>(services);
        }
        
        public async Task<ICollection<ServiceListDto>> GetByProviderIdAsync(string userId)
        {
            try 
            {
                // Add debug logging
                Console.WriteLine($"GetByProviderIdAsync: Looking for services with UserId = {userId}");
                
                var services = await _context.Services
                    .Include(s => s.Provider)
                    .Include(s => s.ServiceSubCategory)
                        .ThenInclude(ssc => ssc.SubCategory)
                            .ThenInclude(sc => sc.Category)
                    .Where(s => s.UserId == userId)
                    .ToListAsync();
                
                Console.WriteLine($"GetByProviderIdAsync: Found {services.Count} services for user {userId}");
                foreach (var service in services)
                {
                    Console.WriteLine($"Service ID: {service.Id}, Title: {service.Title}, SubCategories: {service.ServiceSubCategory?.Count ?? 0}");
                }
                
                // Map to DTOs with explict subcategory mapping
                var serviceDtos = _mapper.Map<List<ServiceListDto>>(services);
                
                // Manually ensure subcategories are mapped correctly if needed
                for(int i = 0; i < services.Count; i++)
                {
                    if (i < serviceDtos.Count && services[i].ServiceSubCategory != null)
                    {
                        // Ensure the DTO has subcategories
                        if (serviceDtos[i].SubCategories == null)
                        {
                            serviceDtos[i].SubCategories = new List<Dto.ServiceSubCategory.ServiceSubCategoryDto>();
                        }
                        
                        // Log what we're doing
                        Console.WriteLine($"Mapping subcategories for service {services[i].Id} - {services[i].ServiceSubCategory.Count} subcategories found");
                        
                        // Manual mapping if needed
                        if (serviceDtos[i].SubCategories.Count == 0 && services[i].ServiceSubCategory.Count > 0)
                        {
                            foreach (var sc in services[i].ServiceSubCategory)
                            {
                                if (sc.SubCategory != null)
                                {
                                    serviceDtos[i].SubCategories.Add(new Dto.ServiceSubCategory.ServiceSubCategoryDto
                                    {
                                        Id = sc.SubCategory.Id,
                                        SubCategoryId = sc.SubCategory.Id,
                                        Name = sc.SubCategory.Name,
                                        SubCategoryName = sc.SubCategory.Name,
                                        CategoryId = sc.SubCategory.CategoryId,
                                        CategoryName = sc.SubCategory.Category?.Name ?? string.Empty
                                    });
                                }
                            }
                        }
                    }
                }
                
                Console.WriteLine($"GetByProviderIdAsync: Returning {serviceDtos.Count} ServiceListDto objects");
                return serviceDtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByProviderIdAsync: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }
        
        public async Task<ICollection<ServiceListDto>> GetAllForAdminAsync()
        {
            var services = await _context.Services
                .Include(s => s.Provider)
                .Include(s => s.ServiceSubCategory)
                    .ThenInclude(ssc => ssc.SubCategory)
                        .ThenInclude(sc => sc.Category)
                .ToListAsync();

            return _mapper.Map<List<ServiceListDto>>(services);
        }

        public async Task<ICollection<ServiceListDto>> GetBySubCategoryIdAsync(int subCategoryId)
        {
            var services = await _context.Services
                .Include(s => s.Provider)
                .Include(s => s.ServiceSubCategory)
                    .ThenInclude(ssc => ssc.SubCategory)
                        .ThenInclude(sc => sc.Category)
                .Where(s => s.ServiceSubCategory.Any(ssc => ssc.SubCategoryId == subCategoryId) && s.IsApproved) // Only return approved services
                .ToListAsync();

            return _mapper.Map<List<ServiceListDto>>(services);
        }

        public async Task<Models.Service> CreateAsync(CreateServiceDto serviceDto, string userId)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = await _context.Users.FindAsync(userId) ?? throw new Exception("User not found");
                var service = _mapper.Map<Models.Service>(serviceDto);
                service.UserId = userId;
                service.Provider = user;
                service.CreatedDate = DateTime.UtcNow;
                service.IsApproved = false; // New services need approval

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

                // Detach all entities from the context to avoid circular references
                foreach (var entry in _context.ChangeTracker.Entries())
                {
                    entry.State = EntityState.Detached;
                }

                // Return the service without loading relationships to avoid circular references
                return new Models.Service
                {
                    Id = service.Id,
                    Title = service.Title,
                    Description = service.Description,
                    EstimatedPrice = service.EstimatedPrice,
                    ContactPhone = service.ContactPhone,
                    CreatedDate = service.CreatedDate,
                    IsApproved = service.IsApproved,
                    UserId = service.UserId
                };
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
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var service = await _context.Services.FindAsync(id);
                if (service == null) return false;

                // Update basic service properties
                _mapper.Map(serviceDto, service);
                
                // Remove existing subcategory relationships
                var existingRelationships = await _context.ServiceSubCategories
                    .Where(ssc => ssc.ServiceId == id)
                    .ToListAsync();
                
                _context.ServiceSubCategories.RemoveRange(existingRelationships);
                await _context.SaveChangesAsync();
                
                // Add new subcategory relationships
                var serviceSubCategories = serviceDto.SubCategoryIds.Select(subCatId => new ServiceSubCategory
                {
                    ServiceId = service.Id,
                    SubCategoryId = subCatId
                }).ToList();

                await _context.ServiceSubCategories.AddRangeAsync(serviceSubCategories);
                await _context.SaveChangesAsync();
                
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error updating service: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
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
        
        public async Task<bool> ApproveServiceAsync(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return false;
            
            service.IsApproved = true;
            return await _context.SaveChangesAsync() > 0;
        }
        
        public async Task<bool> RevokeServiceApprovalAsync(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
                return false;

            service.IsApproved = false;
            _context.Update(service);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<(double Rating, int Count)> GetServiceRatingAsync(int serviceId)
        {
            var comments = await _context.Comments
                .Where(c => c.ServiceId == serviceId && c.Rating > 0)
                .ToListAsync();
            
            if (!comments.Any()) 
                return (0, 0);
            
            double averageRating = comments.Average(c => c.Rating);
            int ratingCount = comments.Count;
            
            return (averageRating, ratingCount);
        }

        public async Task<ICollection<ServiceListDto>> GetByCategoryIdAsync(int categoryId)
        {
            var services = await _context.Services
                .Include(s => s.Provider)
                .Include(s => s.ServiceSubCategory)
                    .ThenInclude(ssc => ssc.SubCategory)
                        .ThenInclude(sc => sc.Category)
                .Where(s => s.ServiceSubCategory.Any(ssc => ssc.SubCategory.CategoryId == categoryId) && s.IsApproved)
                .ToListAsync();

            return _mapper.Map<List<ServiceListDto>>(services);
        }
    }
}
