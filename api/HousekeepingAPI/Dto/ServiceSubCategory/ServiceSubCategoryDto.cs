namespace HousekeepingAPI.Dto.ServiceSubCategory
{
    public class ServiceSubCategoryDto
    {
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        
        // Adding properties to match what's being used in ServiceRepository.cs
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
    }
}
