using IssueTracker.Services;

namespace IssueTracker.Infrastructure
{
    public class EfUnitOfWork(AppDbContext _dbContext) : IUnitOfWork
    {
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}