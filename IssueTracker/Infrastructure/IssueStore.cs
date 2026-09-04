using IssueTracker.Domain;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;

namespace IssueTracker.Infrastructure
{
    public class IssueStore(AppDbContext _dbContext)
    {
        public void Add(Issue issue)
        {
            _dbContext.Add(issue);
        }

        public async Task<List<Issue>> GetAllAsync()
        {
            return await _dbContext.Issues.ToListAsync();
        }

        public async Task<Issue?> GetByIdAsync(int id)
        {
            return await _dbContext.Issues.FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}
