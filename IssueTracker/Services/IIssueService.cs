using IssueTracker.Shared;

namespace IssueTracker.Services;

public interface IIssueService
{
    Task<Result> DeleteIssueAsync(int id);

    Task<Result<CreateIssueOutput>> CreateIssueAsync(
        CreateIssueInput request);

    Task<IReadOnlyList<GetIssueOutput>> GetAllIssuesAsync(
        CancellationToken cancellationToken);

    Task<GetIssueOutput?> GetByIdAsync(int id);

    Task<Result<UpdateIssueOutput>> UpdateIssueAsync(
        int id,
        UpdateIssueInput request);
}