using System.Text.Json.Serialization;

namespace HousekeepingAPI.Models
{
    public class ServiceSubCategory
    {
        public int ServiceId { get; set; }
        public int SubCategoryId { get; set; }
        
        [JsonIgnore]
        public Service? Service { get; set; }
        
        public SubCategory? SubCategory { get; set; }
    }
}
