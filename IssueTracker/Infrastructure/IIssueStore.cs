using IssueTracker.Domain;

namespace IssueTracker.Infrastructure
{
    public interface IIssueStore
    {
        void Add(Issue issue);
        Task<List<Issue>> GetAllAsync();
        Task<Issue?> GetByIdAsync(int id);
        void Delete(Issue issue);

    }
}
