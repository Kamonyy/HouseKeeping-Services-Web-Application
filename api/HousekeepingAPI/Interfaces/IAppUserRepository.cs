using HousekeepingAPI.Models;

namespace HousekeepingAPI.Repository
{
    public interface IAppUserRepository
    {
        Task<AppUser?> GetUserAsync(string userId);
    }
}
