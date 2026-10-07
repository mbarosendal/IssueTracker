using IssueTracker.Services;

namespace IssueTracker.Infrastructure
{
    public class EfUnitOfWork(AppDbContext _dbContext) : IUnitOfWork
    {
        public Task SaveChangesAsync(CancellationToken cancellationToken) =>
            _dbContext.SaveChangesAsync(cancellationToken);
    }
}