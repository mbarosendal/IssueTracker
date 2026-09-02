using Azure;
using IssueTracker.Controllers;
using IssueTracker.Domain;
using IssueTracker.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Threading.Tasks;

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
            var issue = new Issue
            (
                request.Title,
                request.Description,
                DateTimeOffset.UtcNow,
                IssueStatus.Open
            );

            store.AddAsync(issue);

            // is not taking the output from the created? is assumption that exceptions wouldnt let output be made this way?
            var output = new CreateIssueOutput(
                issue.Id,
                issue.Title,
                issue.Description,
                issue.CreatedAt,
                issue.Status
            );

            await unitOfWork.SaveChangesAsync();

            return output;
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

        public async Task<UpdateIssueOutput?> UpdateIssueAsync(int id, UpdateIssueInput update)
        {
            var issue = await store.GetByIdAsync(id);

            if (issue is null)
            {
                return null;
            }

            // EF tracks issue so it will be updated when SaveChangesAsync is called
            issue.Update(
                update.Title,
                update.Description,
                update.Status
                );

            await unitOfWork.SaveChangesAsync();

            return new UpdateIssueOutput(
                issue.Id,
                issue.Title,
                issue.Description,
                issue.CreatedAt,
                issue.UpdatedAt,
                issue.Status
            );
        }
    }
}
