namespace HousekeepingAPI.Dto.Service
{
    public class UpdateServiceDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal EstimatedPrice { get; set; }
        public string ContactPhone { get; set; }
        public List<int> SubCategoryIds { get; set; }
    }
}
