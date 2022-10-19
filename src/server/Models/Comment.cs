namespace V.TouristGuide.Server.Models
{
    public class Comment
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public string Content { get; set; }

        public string Name { get; set; }

        public string Avatar { get; set; }
    }
}
