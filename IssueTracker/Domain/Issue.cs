using System.ComponentModel.DataAnnotations;

namespace IssueTracker.Domain
{

    public sealed class Issue(string title, string description, DateTimeOffset createdAt, IssueStatus status)
    {
        public int Id { get; init; }
        [MaxLength(50)]
        public string Title { get; init; }
        [MaxLength(500)]
        public string Description { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public IssueStatus Status { get; init; }

        public Issue(string title, string description, DateTimeOffset createdAt, IssueStatus status)
        {
            Title = title;
            Description = description;
            CreatedAt = createdAt;
            Status = status;
        }
    }
}
