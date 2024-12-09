namespace HousekeepingAPI.Models
{
    public class ServiceSubCategory
    {

        public int ServiceId { get; set; }
        public Service Service { get; set; } = new Service();

        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; } = new SubCategory();
    }
}
