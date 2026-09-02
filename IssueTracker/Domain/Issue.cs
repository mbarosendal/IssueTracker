using System.ComponentModel.DataAnnotations;

namespace IssueTracker.Domain
{

    public sealed class Issue(string title, string description, DateTimeOffset createdAt, IssueStatus status)
    {
        public int Id { get; init; }
        [MaxLength(50)]
        public string Title { get; private set; } = title;
        [MaxLength(500)]
        public string Description { get; private set; } = description;
        public DateTimeOffset CreatedAt { get; init; } = createdAt;
        public DateTimeOffset? UpdatedAt { get; private set; }
        public IssueStatus Status { get; private set; } = status;

        public void Update(
            string title,
            string description,
            IssueStatus status)
        {
            Title = title;
            Description = description;
            Status = status;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

    }
}
