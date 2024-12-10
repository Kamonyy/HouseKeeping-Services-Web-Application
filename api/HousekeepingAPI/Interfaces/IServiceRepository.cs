using HousekeepingAPI.Dto.Service;
using HousekeepingAPI.Models;

namespace HousekeepingAPI.Interfaces
{
    public interface IServiceRepository
    {
        Task<ICollection<Models.Service>> GetAllAsync();
        Task<Models.Service?> GetByIdAsync(int id);
        Task<List<ServiceSubCategory>> CreateAsync(Models.Service service, List<int> subCategoryIds);
    }
}
