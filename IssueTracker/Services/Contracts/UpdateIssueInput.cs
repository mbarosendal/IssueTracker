using IssueTracker.Domain;

namespace IssueTracker.Services.Contracts
{
    public sealed record UpdateIssueInput(string Title, string Description, IssueStatus Status);
}