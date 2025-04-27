using HousekeepingAPI.Dto.ServiceSubCategory;

namespace HousekeepingAPI.Dto.Service
{
    public class ServiceDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ProviderId { get; set; } = string.Empty;
        public string ProviderUsername { get; set; } = string.Empty;
        public decimal EstimatedPrice { get; set; }
        public string ContactPhone { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public List<ServiceSubCategoryDto> SubCategories { get; set; } = new();
    }
}