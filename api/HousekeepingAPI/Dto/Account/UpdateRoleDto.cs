using System.ComponentModel.DataAnnotations;

namespace HousekeepingAPI.Dto.Account
{
    public class UpdateRoleDto
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Role { get; set; }
    }
} 