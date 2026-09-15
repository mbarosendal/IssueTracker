using IssueTracker.Domain;

namespace IssueTracker.Services.Contracts
{
    public sealed record CreateIssueOutput(
        int Id, string Title, string Description, DateTimeOffset CreatedAt, IssueStatus Status);
}