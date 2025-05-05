using HousekeepingAPI.Models;

namespace HousekeepingAPI.Interfaces
{
    public interface ICategoryRepository
    {
        Task<ICollection<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category?> CreateAsync(Category category);
        Task<Category?> UpdateAsync(int id, Category category);
        Task<Category?> DeleteAsync(int id);
    }
}
