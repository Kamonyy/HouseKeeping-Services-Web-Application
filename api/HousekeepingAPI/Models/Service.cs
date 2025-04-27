using System.ComponentModel.DataAnnotations.Schema;

namespace HousekeepingAPI.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public AppUser? Provider { get; set; }


        public decimal EstimatedPrice { get; set; }
        public string ContactPhone { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public ICollection<ServiceSubCategory> ServiceSubCategory { get; set; } = new List<ServiceSubCategory>();

    }
}