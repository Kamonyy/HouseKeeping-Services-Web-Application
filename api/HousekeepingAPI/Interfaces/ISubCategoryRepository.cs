using HousekeepingAPI.Models;

namespace HousekeepingAPI.Interfaces
{
    public interface ISubCategoryRepository
    {
        Task<ICollection<SubCategory>> GetAllAsync();
        Task<ICollection<SubCategory>?> GetAllByCategoryAsync(int categoryId);
        Task<SubCategory?> GetByIdAsync (int id);
        Task<ICollection<SubCategory>> GetAllByIdsAsync(IEnumerable<int> ids);
        Task<SubCategory> CreateAsync(SubCategory subCategory);
        Task<SubCategory?> UpdateAsync(int id, SubCategory subCategory);
        Task<SubCategory?> DeleteAsync (int id);
    }
}
