using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HousekeepingAPI.Dto.Account
{
    public class RegisterDto
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        [JsonPropertyName("firstName")]
        public string? FirstName { get; set; }

        [Required]
        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }
    }
}
