namespace HousekeepingAPI.Dto.Service
{
    public class ServiceListDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ProviderUsername { get; set; } = string.Empty;
        public decimal EstimatedPrice { get; set; }
    }
}