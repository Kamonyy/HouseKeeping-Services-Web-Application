namespace HousekeepingAPI.Dto.Comment
{
    public class CommentCreateDto
    {
        public string Content { get; set; } = string.Empty;
        public int ServiceId { get; set; }
        public int Rating { get; set; } = 0;
    }
}