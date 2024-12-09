using HousekeepingAPI.Models;

namespace HousekeepingAPI.Interfaces
{
    public interface ISubCategoryRepository
    {
        ICollection<SubCategory> GetAll();
        ICollection<SubCategory> GetAllByCategoty(int CategoryId);
    }
}
