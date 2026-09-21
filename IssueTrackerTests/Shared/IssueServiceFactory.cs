using IssueTracker;
using IssueTracker.Infrastructure;
using IssueTracker.Services;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using Moq;
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
            var logger = Mock.Of<Logger<IssueService>>();
            IssueService issueService = new(issueStore, efUnitOfWork, logger);

            return issueService;
        }
    }
}
