using System.ComponentModel.DataAnnotations;

namespace HousekeepingAPI.Dto.Account
{
    public class LoginDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        
    }
}
