using IssueTracker.Domain;

namespace IssueTracker.Services.Contracts
{
    public sealed record UpdateIssueOutput(
        int Id, string Title, string Description, DateTimeOffset CreatedAt, DateTimeOffset? UpdatedAt, IssueStatus Status);
}