using HousekeepingAPI.Data;
using HousekeepingAPI.Models;

namespace HousekeepingAPI.Repository
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly ApplicationDbContext _context;

        public AppUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AppUser?> GetUserAsync(string userId)
        {
            return await _context.Users.FindAsync(userId);
        }
    }
}
