using HousekeepingAPI.Dto.SubCategory;

namespace HousekeepingAPI.Dto.Service
{
    public class ServiceDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
