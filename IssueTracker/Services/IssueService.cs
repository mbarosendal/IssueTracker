using IssueTracker.Domain;
using IssueTracker.Infrastructure;
using IssueTracker.Shared;

namespace IssueTracker.Services
{
    public sealed record CreateIssueInput(string Title, string Description);

    public sealed record CreateIssueOutput(
        int Id, string Title, string Description, DateTimeOffset CreatedAt, IssueStatus Status);

    public sealed record GetIssueOutput(
        int Id, string Title, string Description, DateTimeOffset CreatedAt, DateTimeOffset? UpdatedAt, IssueStatus Status);

    public sealed record UpdateIssueInput(string Title, string Description, IssueStatus Status);

    public sealed record UpdateIssueOutput(
        int Id, string Title, string Description, DateTimeOffset CreatedAt, DateTimeOffset? UpdatedAt, IssueStatus Status);


    public sealed class IssueService(IIssueRepository store, IUnitOfWork unitOfWork)
    {

        public async Task<Result> DeleteIssueAsync(int id)
        {
            var issue = await store.GetByIdAsync(id);

            if (issue is null)
                return Result.Failure(IssueErrors.NotFound(id));

            store.Delete(issue);

            await unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<CreateIssueOutput>> CreateIssueAsync(CreateIssueInput request)
        {
            var result = Issue.Create(request.Title, request.Description, IssueStatus.Open);

            if (result.IsFailure)
                return Result<CreateIssueOutput>.Failure(result.Error);

            var issue = result.Value;
            store.Add(issue);
            await unitOfWork.SaveChangesAsync(); // populates issue.Id

            var output = new CreateIssueOutput(
                issue.Id, issue.Title, issue.Description, issue.CreatedAt, issue.Status);

            return Result<CreateIssueOutput>.Success(output);
        }

        public async Task<IReadOnlyList<GetIssueOutput>> GetAllIssuesAsync(CancellationToken cancellationToken)
        {
            var issues = await store.GetAllAsync(cancellationToken);

            return issues
                .Select(i => new GetIssueOutput(i.Id, i.Title, i.Description, i.CreatedAt, i.UpdatedAt, i.Status))
                .ToList();
        }

        public async Task<GetIssueOutput?> GetByIdAsync(int id)
        {
            var issue = await store.GetByIdAsync(id);

            if (issue is null) 
                return null;

            return new GetIssueOutput(
                issue.Id, issue.Title, issue.Description, issue.CreatedAt, issue.UpdatedAt, issue.Status);
        }

        public async Task<Result<UpdateIssueOutput>> UpdateIssueAsync(int id, UpdateIssueInput request)
        {
            var issue = await store.GetByIdAsync(id);
            if (issue is null)
                return Result<UpdateIssueOutput>.Failure(IssueErrors.NotFound(id));

            var result = issue.Update(request.Title, request.Description, request.Status);
            if (result.IsFailure)
                return Result<UpdateIssueOutput>.Failure(result.Error);

            await unitOfWork.SaveChangesAsync();

            var output = new UpdateIssueOutput(
                issue.Id, issue.Title, issue.Description, issue.CreatedAt, issue.UpdatedAt, issue.Status);

            return Result<UpdateIssueOutput>.Success(output);
        }
    }
}