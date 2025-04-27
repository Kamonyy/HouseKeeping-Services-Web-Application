using HousekeepingAPI.Dto.Service;
namespace HousekeepingAPI.Interfaces
{
    public interface IServiceRepository
    {
        Task<ServiceDetailsDto> GetByIdAsync(int id);
        Task<Models.Service> GetServiceById(int id);

        Task<ICollection<ServiceListDto>> GetAllAsync();
        Task<Models.Service> CreateAsync(CreateServiceDto serviceDto, string userId);
        Task<bool> UpdateAsync(int id, UpdateServiceDto serviceDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> Exists(int id);
    }
}