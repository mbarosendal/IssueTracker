using IssueTracker.Domain;
using IssueTracker.Infrastructure;
using IssueTracker.Services;
using IssueTracker.Services.Contracts;
using IssueTracker.Services.Errors;
using IssueTracker.Shared;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace IssueTrackerTests.Tests.Unit.Services
{
    [TestClass]
    public class IssueServiceTests
    {
        [TestMethod]
        public async Task UpdateIssueAsync_WhenConcurrencyConflictOccurs_ReturnsConflict()
        {
            // Arrange
            var resultIssue = Issue.Create(
                "testTitle", 
                "testDescription.", 
                IssueStatus.Open);

            var issue = resultIssue.Value;

            Mock<IIssueRepository> storeMock = new();
            storeMock
                .Setup(x => x.GetByIdAsync(123))
                .ReturnsAsync(issue);

            Mock<IUnitOfWork> unitMock = new();
            unitMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.FromException(new DbUpdateConcurrencyException()));

            IssueService issueService = new(storeMock.Object, unitMock.Object);

            UpdateIssueInput updateInput = new(
                "updateTitle", 
                "updateDescription", 
                IssueStatus.InProgress);

            // Act
            var updateResult = await issueService.UpdateIssueAsync(123, updateInput);

            // Assert
            Assert.IsTrue(updateResult.IsFailure);
            Assert.AreEqual(ErrorType.Conflict, updateResult.Error.Type);
            Assert.AreEqual(IssueServiceErrors.ConcurrencyConflict, updateResult.Error);
        }

        [TestMethod()]
        public async Task DeleteIssueAsync_WhenIssueExists_ReturnsSuccess()
        {
            // Arrange
            var resultIssue = Issue.Create(
                "testTitle", 
                "testDescription.", 
                IssueStatus.Open);

            var issue = resultIssue.Value;

            Mock<IIssueRepository> storeMock = new();
            storeMock
                .Setup(x => x.GetByIdAsync(123))
                .ReturnsAsync(issue);
            Mock<IUnitOfWork> unitMock = new();
            unitMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            IssueService service = new(storeMock.Object, unitMock.Object);

            // Act
            var result = await service.DeleteIssueAsync(123);

            // Assert
            storeMock.Verify(x => x.GetByIdAsync(123), Times.Once());
            storeMock.Verify(x => x.Delete(issue), Times.Once());
            unitMock.Verify(x => x.SaveChangesAsync(), Times.Once());
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod()]
        public async Task DeleteIssueAsync_WhenIssueDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            Mock<IIssueRepository> storeMock = new();
            storeMock
                .Setup(x => x.GetByIdAsync(123))
                .ReturnsAsync((Issue?)null);

            Mock<IUnitOfWork> unitMock = new();

            IssueService service = new(storeMock.Object, unitMock.Object);

            // Act
            var result = await service.DeleteIssueAsync(123);

            // Assert
            storeMock.Verify(x => x.GetByIdAsync(123), Times.Once());
            unitMock.Verify(x => x.SaveChangesAsync(), Times.Never());
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual(ErrorType.NotFound, result.Error.Type);
        }

        [TestMethod()]
        public async Task DeleteIssueAsync_WhenPersistenceFails_PropagatesException()
        {
            // Arrange
            var resultIssue = Issue.Create(
            "testTitle",
            "TestDescription",
            IssueStatus.Open);

            var issue = resultIssue.Value;

            Mock<IIssueRepository> storeMock = new();
            storeMock
                .Setup(x => x.GetByIdAsync(123))
                .ReturnsAsync(issue);

            Mock<IUnitOfWork> unitMock = new();
            unitMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.FromException(new Exception()));

            IssueService service = new(storeMock.Object, unitMock.Object);

            // Assert
            await Assert.ThrowsExceptionAsync<Exception>(
                () => service.DeleteIssueAsync(123));

            storeMock.Verify(x => x.GetByIdAsync(123), Times.Once());
            unitMock.Verify(x => x.SaveChangesAsync(), Times.Once());
        }
    }
}
