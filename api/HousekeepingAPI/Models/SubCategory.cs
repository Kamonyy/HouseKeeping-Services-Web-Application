using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HousekeepingAPI.Models
{
    public class SubCategory
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        
        [JsonIgnore]
        public Category Category { get; set; } = new Category();
        
        [JsonIgnore]
        public ICollection<ServiceSubCategory> ServiceSubCategory { get; set; } = new List<ServiceSubCategory>();

    }
}
