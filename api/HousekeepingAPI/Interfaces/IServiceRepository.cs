using HousekeepingAPI.Dto.Service;
namespace HousekeepingAPI.Interfaces
{
    public interface IServiceRepository
    {
        Task<ServiceDetailsDto> GetByIdAsync(int id);
        Task<Models.Service> GetServiceById(int id);

        Task<ICollection<ServiceListDto>> GetAllAsync();
        Task<ICollection<ServiceListDto>> GetBySubCategoryIdAsync(int subCategoryId);
        Task<ICollection<ServiceListDto>> GetByCategoryIdAsync(int categoryId);
        Task<ICollection<ServiceListDto>> GetByProviderIdAsync(string userId);
        Task<ICollection<ServiceListDto>> GetAllForAdminAsync();
        Task<Models.Service> CreateAsync(CreateServiceDto serviceDto, string userId);
        Task<bool> UpdateAsync(int id, UpdateServiceDto serviceDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> Exists(int id);
        Task<bool> ApproveServiceAsync(int id);
        Task<bool> RevokeServiceApprovalAsync(int id);
        Task<(double Rating, int Count)> GetServiceRatingAsync(int serviceId);
    }
}