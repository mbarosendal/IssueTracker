using IssueTracker;
using Microsoft.EntityFrameworkCore;

namespace IssueTrackerTests
{
    public sealed class IntegrationTestDatabase(string connectionString)
    {
        public async Task<AppDbContext> CreateContextAsync()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            var dbContext = new AppDbContext(options);

            await dbContext.Database.MigrateAsync();

            return dbContext;
        }
    }
}
