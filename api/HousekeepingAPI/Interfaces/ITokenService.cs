using HousekeepingAPI.Models;

namespace HousekeepingAPI.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
