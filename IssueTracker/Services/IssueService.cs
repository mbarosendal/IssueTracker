using Azure;
using IssueTracker.Controllers;
using IssueTracker.Domain;
using IssueTracker.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Threading.Tasks;
using static IssueTracker.Domain.Result;

namespace IssueTracker.Services
{

    public sealed record CreateIssueInput(
        string Title,
        string Description
        );

    public sealed record CreateIssueOutput(
        int Id,
        string Title,
        string Description,
        DateTimeOffset CreatedAt,
        IssueStatus Status
        );

    public sealed record GetIssueOutput(
        int Id,
        string Title,
        string Description,
        DateTimeOffset CreatedAt,
        DateTimeOffset? UpdatedAt,
        IssueStatus Status
        );

    public sealed record UpdateIssueInput(
        string Title,
        string Description,
        IssueStatus Status
        );

    public sealed record UpdateIssueOutput(
        int Id,
        string Title,
        string Description,
        DateTimeOffset CreatedAt,
        DateTimeOffset? UpdatedAt,
        IssueStatus Status
        );

    public sealed class IssueService(IssueStore store, IUnitOfWork unitOfWork)
    {

        public async Task<CreateIssueOutput> CreateIssueAsync(CreateIssueInput request)
        {
            var result = Issue.Create(
                request.Title,
                request.Description,
                IssueStatus.Open);

            if (result is Result<Issue>.Failure failure)
            {
                // Translate the domain failure into your application outcome.
                // We'll wire this to the controller's 400 response.
                throw new NotImplementedException();
            }

            var success = (Result<Issue>.Success)result;
            var issue = success.Value;

            store.Add(issue);

            await unitOfWork.SaveChangesAsync();

            // construct the output after SaveChangesAsync(), because that's when the database-generated Id has been populated.
            return new CreateIssueOutput(
                issue.Id,
                issue.Title,
                issue.Description,
                issue.CreatedAt,
                issue.Status);
        }

        public async Task<IReadOnlyList<GetIssueOutput>> GetAllIssuesAsync()
        {
            var result = await store.GetAllAsync();

            List<GetIssueOutput> outputList = new();

            foreach (var issue in result)
            {
                var output = new GetIssueOutput(
                    issue.Id,
                    issue.Title,
                    issue.Description,
                    issue.CreatedAt,
                    issue.UpdatedAt,
                    issue.Status
                );
                outputList.Add(output);
            }

            return outputList;

        }

        public async Task<GetIssueOutput?> GetByIdAsync(int id)
        {
            var result = await store.GetByIdAsync(id);

            if (result is null)
            {
                return null;
            }

            var output = new GetIssueOutput(
                result.Id,
                result.Title,
                result.Description,
                result.CreatedAt,
                result.UpdatedAt,
                result.Status
            );

            return output;
        }

        public async Task<UpdateIssueOutput?> UpdateIssueAsync(
            int id,
            UpdateIssueInput request)
        {
            var issue = await store.GetByIdAsync(id);

            if (issue is null)
                return null;

            var result = issue.Update(
                request.Title,
                request.Description,
                request.Status);

            if (result is Result.InvalidTitle)
            {
                // Translate to application failure.
                throw new NotImplementedException();
            }

            if (result is Result.InvalidDescription)
            {
                // Translate to application failure.
                throw new NotImplementedException();
            }

            await unitOfWork.SaveChangesAsync();

            return new UpdateIssueOutput(
                issue.Id,
                issue.Title,
                issue.Description,
                issue.CreatedAt,
                issue.UpdatedAt,
                issue.Status);
        }
    }
}
