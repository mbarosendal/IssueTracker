using IssueTracker.Domain;

namespace IssueTracker.Infrastructure
{
    public interface IIssueRepository
    {
        void Add(Issue issue);
        Task<List<Issue>> GetAllAsync(CancellationToken cancellationToken);
        Task<Issue?> GetByIdAsync(int id);
        void Delete(Issue issue);

    }
}
