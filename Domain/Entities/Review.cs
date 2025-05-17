namespace Domain.Entities
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int Rate { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
