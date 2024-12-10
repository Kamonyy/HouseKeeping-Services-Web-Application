namespace HousekeepingAPI.Models
{
    public class ServiceSubCategory
    {
        public int ServiceId { get; set; }
        public int SubCategoryId { get; set; }
        public Service? Service { get; set; }
        public SubCategory? SubCategory { get; set; }
    }
}
