using IssueTracker.Domain;
using System.Security.Cryptography.Xml;

namespace IssueTracker.Infrastructure
{
    public class IssueStore
    {
        private AppDbContext _dbContext;

        public IssueStore(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Issue issue)
        {
            _dbContext.Add(issue);
        }

        public List<Issue> GetAll()
        {
            return _dbContext.Issues.ToList();
        }

        public Issue? GetById(int id)
        {
            return _dbContext.Issues.FirstOrDefault(i => i.Id == id);
        }
    }
}
