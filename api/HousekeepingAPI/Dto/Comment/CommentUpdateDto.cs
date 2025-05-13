namespace HousekeepingAPI.Dto.Comment
{
    public class CommentUpdateDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public int ServiceId { get; set; }
        public int Rating { get; set; } = 0;
    }
}