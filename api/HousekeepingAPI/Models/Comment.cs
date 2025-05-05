namespace HousekeepingAPI.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public int ServiceId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public virtual AppUser User { get; set; }
        public virtual Service Service { get; set; }
    }
}
