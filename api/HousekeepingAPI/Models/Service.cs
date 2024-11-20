namespace HousekeepingAPI.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public AppUser User { get; set; } = new AppUser();
        public int CategoryId { get; set; }
        public Category Category { get; set; } = new Category();
    }
}
