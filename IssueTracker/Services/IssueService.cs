using IssueTracker.Controllers;
using IssueTracker.Domain;
using IssueTracker.Infrastructure;
using System.Threading.Tasks;

namespace IssueTracker.Services
{

    public sealed record CreateIssueInput(
        string Title,
        string Description);

    public sealed record CreateIssueOutput(
        int Id,
        string Title,
        string Description,
        DateTimeOffset CreatedAt,
        IssueStatus Status);

    public sealed record GetIssueOutput(
        int Id,
        string Title,
        string Description,
        DateTimeOffset CreatedAt,
        IssueStatus Status);
    // could you make a IssueOutput combined here?

    public sealed class IssueService(IssueStore store, IUnitOfWork unitOfWork)
    {

        public async Task<CreateIssueOutput> CreateIssue(CreateIssueInput request)
        {
            var issue = new Issue
            (
                "request.Titlerequest.Titlerequest.Titlerequest.Titlerequest.Titlerequest.Titlerequest.Titlerequest.Titlerequest.Titlerequest.Title",
                request.Description,
                DateTimeOffset.UtcNow,
                IssueStatus.Open
            ); 

            store.Add(issue);

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

        public IReadOnlyList<GetIssueOutput> GetAllIssues()
        {
            var result = store.GetAll();

            List<GetIssueOutput> outputList = new List<GetIssueOutput>();

            foreach (var issue in result)
            {
                var output = new GetIssueOutput(
                    issue.Id,
                    issue.Title,
                    issue.Description,
                    issue.CreatedAt,
                    issue.Status
                );
                outputList.Add(output);
            }

            return outputList;

        }

        public GetIssueOutput? GetById(int id)
        {
            var result = store.GetById(id);

            if (result is null)
            {
                return null;
            }

            var output = new GetIssueOutput(
                result.Id,
                result.Title,
                result.Description,
                result.CreatedAt,
                result.Status
            );

            return output;
        }



    }
}
