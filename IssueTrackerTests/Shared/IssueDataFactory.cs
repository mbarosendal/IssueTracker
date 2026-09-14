using IssueTracker;
using IssueTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTrackerTests.Shared
{
    internal class IssueDataFactory
    {
        public static async Task<Issue> CreateAsync(
            AppDbContext dbContext, 
            string title, 
            string description, 
            IssueStatus status)
        {
            var issue = Issue.Create(
                title, 
                description, 
                status);

            dbContext.Issues.Add(issue.Value);
            await dbContext.SaveChangesAsync();

            return issue.Value;
        }
    }
}
