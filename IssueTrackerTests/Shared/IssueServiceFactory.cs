using IssueTracker;
using IssueTracker.Infrastructure;
using IssueTracker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTrackerTests.Shared
{
    internal class IssueServiceFactory
    {
        public static IssueService CreateIssueService(AppDbContext context)
        {
            IssueRepository issueStore = new(context);
            EfUnitOfWork efUnitOfWork = new(context);
            IssueService issueService = new(issueStore, efUnitOfWork);

            return issueService;
        }
    }
}
