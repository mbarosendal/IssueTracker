namespace IssueTracker.Domain
{

    public sealed class Issue(string title, string description, DateTimeOffset createdAt, IssueStatus status)
    {
        public int Id { get; init; }
        public string Title { get; init; } = title;
        public string Description { get; init; } = description;
        public DateTimeOffset CreatedAt { get; init; } = createdAt;
        public IssueStatus Status { get; init; } = status;
    }
}
