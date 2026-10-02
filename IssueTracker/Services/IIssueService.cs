using IssueTracker.Services.Contracts;
using IssueTracker.Shared;

namespace IssueTracker.Services;

public interface IIssueService
{
    Task<Result> DeleteIssueAsync(int id, 
        CancellationToken cancellationToken);

    Task<Result<CreateIssueOutput>> CreateIssueAsync(
        CreateIssueInput request,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<GetIssueOutput>> GetAllIssuesAsync(
        CancellationToken cancellationToken);

    Task<GetIssueOutput?> GetByIdAsync(int id, 
        CancellationToken cancellationToken);

    Task<Result<UpdateIssueOutput>> UpdateIssueAsync(
        int id,
        UpdateIssueInput request, 
        CancellationToken cancellationToken);
}