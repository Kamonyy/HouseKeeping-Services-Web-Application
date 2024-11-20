using HousekeepingAPI.Models;

namespace HousekeepingAPI.Dto
{
    public class ServiceDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public int CategoryId { get; set; }
    }
}
