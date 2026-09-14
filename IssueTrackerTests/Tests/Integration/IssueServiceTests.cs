using Docker.DotNet.Models;
using IssueTracker.Domain;
using IssueTracker.Services;
using IssueTrackerTests.Shared;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

namespace IssueTrackerTests.Tests.Integration
{
    [TestClass]
    public class IssueServiceTests : DatabaseFixture
    {
        [TestMethod]
        public async Task UpdateIssue_WhenRowVersionIsStale_ThrowsConcurrencyException()
        {
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

            //Act
            firstIssue.Update("newTitle", "newDescription", IssueStatus.InProgress);
            await firstContext.SaveChangesAsync();

            secondIssue.Update("newTitleTwo", "newDescriptionTwo", IssueStatus.InProgress);

            //Assert
            await Assert.ThrowsExceptionAsync<DbUpdateConcurrencyException>(
                () => secondContext.SaveChangesAsync());
        }


        [TestMethod]
        public async Task UpdateIssue_WhenIssueExists_UpdatesIssue()
        {
            await using var createContext = CreateContext();
            var createdIssue = await IssueDataFactory.CreateAsync(createContext, "testTitle", "testDescription", IssueStatus.Open);

            await using var updateContext = CreateContext();
            var actService = IssueServiceFactory.CreateIssueService(updateContext);
            var updateRequest = new UpdateIssueInput("newTitle", "newDescription", IssueStatus.InProgress);

            //Act
            var actResult = await actService.UpdateIssueAsync(createdIssue.Id, updateRequest);

            //Assert
            await using var verificationContext = CreateContext();

            var assertResult = await verificationContext.Issues
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == createdIssue.Id);

            Assert.IsNotNull(assertResult);
            Assert.IsTrue(actResult.IsSuccess);
            Assert.AreEqual(updateRequest.Title, assertResult.Title);
            Assert.AreEqual(updateRequest.Description, assertResult.Description);
            Assert.AreEqual(updateRequest.Status, assertResult.Status);
        }

        [TestMethod]
        public async Task GetAllIssues_WhenIssuesExist_ReturnsAllIssues()
        {
            await using var createContext = CreateContext();
            await IssueDataFactory.CreateAsync(createContext, "testTitle", "testDescription", IssueStatus.Open);
            await IssueDataFactory.CreateAsync(createContext, "testTitleTwo", "testDescriptionTwo", IssueStatus.Open);

            await using var readContext = CreateContext();
            var service = IssueServiceFactory.CreateIssueService(readContext);

            //Act
            var readResult = await service.GetAllIssuesAsync(CancellationToken.None);

            //Assert
            Assert.IsNotNull(readResult.FirstOrDefault(i => i.Title == "testTitle"));
            Assert.IsNotNull(readResult.FirstOrDefault(i => i.Title == "testTitleTwo"));
            Assert.AreEqual(2, readResult.Count);
        }

        [TestMethod]
        public async Task GetById_WhenIssueExists_ReturnsIssue()
        {
            //Arrange
            await using var createContext = CreateContext();
            var createdIssue = await IssueDataFactory.CreateAsync(createContext, "testTitle", "testDescription", IssueStatus.Open);

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
        public async Task DeleteIssueAsync_WhenIssueExists_DeletesAndSaves()
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

            var readResult = await verificationContext.Issues
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == createdIssue.Id);

            Assert.IsTrue(deleteResult.IsSuccess);
            Assert.IsNull(readResult);
        }

        [TestMethod]
        public async Task AddIssue_WhenSaved_PersistsIssue()
        {
            // Arrange
            await using var createContext = CreateContext();
            var service = IssueServiceFactory.CreateIssueService(createContext);

            var issue = new CreateIssueInput(
                "AddIssue_WhenSaved_PersistsIssue",
                "description"
                );

            // Act
            await service.CreateIssueAsync(issue);

            // Assert
            await using var verificationContext = CreateContext();

            var readResult = await verificationContext.Issues
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Title == "AddIssue_WhenSaved_PersistsIssue");

            Assert.IsNotNull(readResult);
            Assert.IsTrue(readResult.Id > 0);
            Assert.AreNotEqual(default, readResult.CreatedAt);
            Assert.AreEqual(issue.Title, readResult.Title);
            Assert.AreEqual(issue.Description, readResult.Description);
        }
    }
}
