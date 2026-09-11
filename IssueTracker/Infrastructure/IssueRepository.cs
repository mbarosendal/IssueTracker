using IssueTracker.Domain;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;

namespace IssueTracker.Infrastructure
{
    public class IssueRepository(AppDbContext _dbContext) : IIssueRepository
    {
        public void Add(Issue issue)
        {
            _dbContext.Add(issue);
        }

        public async Task<List<Issue>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Issues.ToListAsync(cancellationToken);
        }

        public async Task<Issue?> GetByIdAsync(int id)
        {
            return await _dbContext.Issues.FirstOrDefaultAsync(i => i.Id == id);
        }

        public void Delete(Issue issue)
        {
            _dbContext.Remove(issue);
        }
    }
}
