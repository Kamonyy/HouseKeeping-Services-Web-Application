using HousekeepingAPI.Models;

namespace HousekeepingAPI.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetAll();
        Category GetById(int id);
    }
}
