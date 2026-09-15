using IssueTracker.Domain;

namespace IssueTracker.Controllers.Contracts
{
    public sealed class GetIssueResponse
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset? UpdatedAt { get; init; }
        public IssueStatus Status { get; init; }
    }

}
