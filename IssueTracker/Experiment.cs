using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Experiments
{
    public static class Experiment
    {
        // Fixed
        public static async Task RunNPlusOneExperiment(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Load the relationship together 
            var issues = await db.Issues
                .Include(i => i.Comments)
                .Take(10)
                .ToListAsync();

            // And remove the query from the loop
            foreach (var issue in issues)
            {
                var comments = issue.Comments;

                Console.WriteLine(
                    $"Issue {issue.Id}: loaded {comments.Count} comments.");
            }
        }

        //public static async Task RunNPlusOneExperiment(IServiceProvider services)
        //{
        //    using var scope = services.CreateScope();

        //    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        //    var issues = await db.Issues
        //        .Take(10)
        //        .ToListAsync();

        //    foreach (var issue in issues)
        //    {
        //        var comments = await db.Comments
        //            .Where(c => c.IssueId == issue.Id)
        //            .ToListAsync();
        //    }
        //}
    }
}
