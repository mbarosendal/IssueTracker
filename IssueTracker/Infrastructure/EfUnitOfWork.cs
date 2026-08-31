using IssueTracker.Services;

namespace IssueTracker.Infrastructure
{
    internal class EfUnitOfWork(AppDbContext _dbContext) : IUnitOfWork
    {
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}