using System.ComponentModel.DataAnnotations;

namespace IssueTracker.Domain
{
    public sealed class Comment
    {
        private Comment(string content, DateTimeOffset createdAt)
        {
            Content = content;
            CreatedAt = createdAt;
        }

        public int Id { get; init; }

        [MaxLength(500)] public string Content { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }

        public int IssueId { get; private set; }
        public Issue Issue { get; private set; } = null!;

        public static Comment Create(string content)
        {
            return new Comment(content, DateTimeOffset.UtcNow);
        }
    }
}
