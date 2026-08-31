using IssueTracker.Services;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Infrastructure
{
    public sealed class EfUnitOfWork(AppDbContext dbContext) : IUnitOfWork
    {
        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
