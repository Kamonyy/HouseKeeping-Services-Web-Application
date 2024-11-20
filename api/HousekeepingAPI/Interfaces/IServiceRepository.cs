
namespace HousekeepingAPI.Interfaces
{
    public interface IServiceRepository
    {
        ICollection<Models.Service> GetAll();
        Models.Service GetById(int id);
    }
}
