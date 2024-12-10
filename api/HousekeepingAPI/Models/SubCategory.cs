using System.ComponentModel.DataAnnotations;

namespace HousekeepingAPI.Models
{
    public class SubCategory
    {
        public int id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = new Category();
        public ICollection<ServiceSubCategory> ServiceSubCategory { get; set; } = new List<ServiceSubCategory>();

    }
}
