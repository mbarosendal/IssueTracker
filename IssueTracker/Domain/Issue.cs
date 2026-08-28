namespace IssueTracker.Domain
{

    public sealed class Issue
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public DateTimeOffset CreatedAt { get; init; }
        public IssueStatus Status { get; init; }

        public Issue(int id, string title, string description, DateTimeOffset createdAt, IssueStatus status)
        {
            Id = id;
            Title = title;
            Description = description;
            CreatedAt = createdAt;
            Status = status;
        }
    }
}
