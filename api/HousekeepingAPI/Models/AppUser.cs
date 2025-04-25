using Microsoft.AspNetCore.Identity;

namespace HousekeepingAPI.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateRegistered { get; set; } = DateTime.UtcNow;
        public UserType UserType { get; set; }
    }

    public enum UserType
    {
        Customer,
        Provider,
        Admin
    }
}
