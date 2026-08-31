using System.ComponentModel.DataAnnotations;

namespace IssueTracker.Domain
{

    public sealed class Issue(string title, string description, DateTimeOffset createdAt, IssueStatus status)
    {
        public int Id { get; init; }
        [MaxLength(50)]
        public string Title { get; init; } = title;
        [MaxLength(500)]
        public string Description { get; init; } = description;
        public DateTimeOffset CreatedAt { get; init; } = createdAt;
        public DateTime? UpdatedAt { get; init; }
        public IssueStatus Status { get; init; } = status;
    }
}
