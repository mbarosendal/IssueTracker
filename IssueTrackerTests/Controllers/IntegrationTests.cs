using IssueTracker.Domain;
using IssueTracker.Infrastructure;
using IssueTracker.Services;
using Microsoft.EntityFrameworkCore;

namespace IssueTrackerTests.Controllers
{
    [TestClass]
    public class IntegrationTests : IntegrationTestFixture
    {
        [TestMethod]
        public async Task CanConnectToIntegrationDatabase()
        {
            // Arrange
            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__IntegrationTest");
            var testDatabase = await IntegrationTestDatabase.CreateAsync(connectionString);

            // Act
            await using var context = await testDatabase.CreateContextAsync();

            // Assert
            Assert.IsTrue(await context.Database.CanConnectAsync());
        }

        [TestMethod]
        public async Task DeleteIssueAsync_WhenIssueExists_DeletesAndSaves()
        {
            // Arrange
            await using var context = await Database.CreateContextAsync();

            IssueStore issueStore = new(context);
            EfUnitOfWork efUnitOfWork = new(context);
            IssueService issueService = new(issueStore, efUnitOfWork);

            var result = Issue.Create(title: "TestTitle", description: "TestDescription", IssueStatus.Open);
            var issue = result.Value;
            issueStore.Add(issue);

            await efUnitOfWork.SaveChangesAsync();
            var issueId = issue.Id;

            // Act
            var resultDelete = await issueService.DeleteIssueAsync(issueId);

            await using var newContextPostDelete = await Database.CreateContextAsync();

            var resultFind = await newContextPostDelete.Issues
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == issueId);

            // Assert
            Assert.IsTrue(resultDelete.IsSuccess);
            Assert.IsNull(resultFind);
        }

        [TestMethod]
        public async Task AddIssue_WhenSaved_PersistsIssue()
        {
            // Arrange
            await using var context = await Database.CreateContextAsync();

            IssueStore issueStore = new(context);
            EfUnitOfWork efUnitOfWork = new(context);

            var result = Issue.Create(title: "TestTitle", description: "TestDescription", IssueStatus.Open);
            var issue = result.Value;
            issueStore.Add(issue);
            await efUnitOfWork.SaveChangesAsync();

            // Act
            await using var newContextPostCreate = await Database.CreateContextAsync();

            var resultFind = await newContextPostCreate.Issues
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == issue.Id);

            // Assert
            Assert.IsNotNull(resultFind);
            Assert.AreEqual(issue.Id, resultFind.Id);
            Assert.AreEqual(issue.Title, resultFind.Title);
            Assert.AreEqual(issue.Description, resultFind.Description);
            Assert.AreEqual(issue.CreatedAt, resultFind.CreatedAt);
        }

    }
}
