using HousekeepingAPI.Models;

namespace HousekeepingAPI.Interfaces
{
    public interface ICategoryRepository
    {
        Task<ICollection<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
    }
}
