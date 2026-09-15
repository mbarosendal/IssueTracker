using IssueTracker.Domain;
using IssueTracker.Services.Contracts;
using IssueTrackerTests.Shared;
using Microsoft.EntityFrameworkCore;

namespace IssueTrackerTests.Tests.Integration.Services
{
    [TestClass]
    public class IssueServiceIntegrationTests : DatabaseFixture
    {
        [TestMethod]
        public async Task UpdateIssueAsync_WhenRowVersionIsStale_ThrowsConcurrencyException()
        {
            // Arrange
            await using var createContext = CreateContext();

            var createdIssue = await IssueDataFactory.CreateAsync(
                createContext,
                "Concurrency test",
                "testDescription",
                IssueStatus.Open
                );

            await using var firstContext = CreateContext();
            var firstIssue = await firstContext.Issues.FindAsync(createdIssue.Id);
            Assert.IsNotNull(firstIssue);

            await using var secondContext = CreateContext();
            var secondIssue = await secondContext.Issues.FindAsync(createdIssue.Id);
            Assert.IsNotNull(secondIssue);

            // Act
            firstIssue.Update("newTitle", "newDescription", IssueStatus.InProgress);
            await firstContext.SaveChangesAsync();

            secondIssue.Update("newTitleTwo", "newDescriptionTwo", IssueStatus.InProgress);

            // Assert
            await Assert.ThrowsExceptionAsync<DbUpdateConcurrencyException>(
                () => secondContext.SaveChangesAsync());
        }


        [TestMethod]
        public async Task UpdateIssue_WhenIssueExists_UpdatesIssue()
        {
            // Arrange
            await using var createContext = CreateContext();

            var createdIssue = await IssueDataFactory.CreateAsync(
                createContext, 
                "testTitle", 
                "testDescription", 
                IssueStatus.Open);

            await using var updateContext = CreateContext();

            var service = IssueServiceFactory.CreateIssueService(updateContext);

            var updateRequest = new UpdateIssueInput(
                "newTitle", 
                "newDescription", 
                IssueStatus.InProgress);

            //Act
            var updateResult = await service.UpdateIssueAsync(createdIssue.Id, updateRequest);
            Assert.IsTrue(updateResult.IsSuccess);

            //Assert
            await using var verificationContext = CreateContext();

            var verificationResult = await verificationContext.Issues
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == createdIssue.Id);

            Assert.IsNotNull(verificationResult);            
            Assert.AreEqual(updateRequest.Title, verificationResult.Title);
            Assert.AreEqual(updateRequest.Description, verificationResult.Description);
            Assert.AreEqual(updateRequest.Status, verificationResult.Status);
        }

        [TestMethod]
        public async Task GetAllIssues_WhenIssuesExist_ReturnsAllIssues()
        {
            // Arrange
            await using var createContext = CreateContext();

            await IssueDataFactory.CreateAsync(
                createContext, 
                "testTitle", 
                "testDescription", 
                IssueStatus.Open);

            await IssueDataFactory.CreateAsync(
                createContext, 
                "testTitleTwo", 
                "testDescriptionTwo", 
                IssueStatus.Open);

            await using var readContext = CreateContext();

            var service = IssueServiceFactory.CreateIssueService(readContext);

            //Act
            var readResult = await service.GetAllIssuesAsync(CancellationToken.None);

            //Assert
            Assert.IsNotNull(readResult.Any(i => i.Title == "testTitle"));
            Assert.IsNotNull(readResult.Any(i => i.Title == "testTitleTwo"));
            Assert.AreEqual(2, readResult.Count);
        }

        [TestMethod]
        public async Task GetById_WhenIssueExists_ReturnsIssue()
        {
            //Arrange
            await using var createContext = CreateContext();

            var createdIssue = await IssueDataFactory.CreateAsync(
                createContext, 
                "testTitle", 
                "testDescription", 
                IssueStatus.Open);

            await using var readContext = CreateContext();

            var service = IssueServiceFactory.CreateIssueService(readContext);

            //Act
            var readResult = await service.GetByIdAsync(createdIssue.Id);

            //Assert
            Assert.IsNotNull(readResult);
            Assert.AreEqual(createdIssue.Id, readResult.Id);
            Assert.AreEqual(createdIssue.Description, readResult.Description);
            Assert.AreEqual(createdIssue.Title, readResult.Title);
        }

        [TestMethod]
        public async Task DeleteIssueAsync_WhenIssueExists_DeletesIssue()
        {
            // Arrange
            await using var createContext = CreateContext();

            var createdIssue = await IssueDataFactory.CreateAsync(
                createContext,
                "testTitle",
                "testDescription",
                IssueStatus.Open);

            await using var deleteContext = CreateContext();

            var service = IssueServiceFactory.CreateIssueService(deleteContext);

            // Act
            var deleteResult = await service.DeleteIssueAsync(createdIssue.Id);

            // Assert
            await using var verificationContext = CreateContext();

            var verificationResult = await verificationContext.Issues
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == createdIssue.Id);

            Assert.IsTrue(deleteResult.IsSuccess);
            Assert.IsNull(verificationResult);
        }

        [TestMethod]
        public async Task CreateIssueAsync_WhenIssueIsCreated_PersistsIssue()
        {
            // Arrange
            await using var createContext = CreateContext();

            var service = IssueServiceFactory.CreateIssueService(createContext);

            var issue = new CreateIssueInput(
                "testTitle",
                "testDescription"
                );

            // Act
            await service.CreateIssueAsync(issue);

            // Assert
            await using var verificationContext = CreateContext();

            var verificationResult = await verificationContext.Issues
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Title == "testTitle");

            Assert.IsNotNull(verificationResult);
            Assert.IsTrue(verificationResult.Id > 0);
            Assert.AreNotEqual(default, verificationResult.CreatedAt);
            Assert.AreEqual(issue.Title, verificationResult.Title);
            Assert.AreEqual(issue.Description, verificationResult.Description);
        }
    }
}
