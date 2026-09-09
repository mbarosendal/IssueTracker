using IssueTracker.Domain;
using IssueTracker.Infrastructure;
using IssueTracker.Services;
using IssueTracker.Shared;
using Moq;

namespace IssueTrackerTests.Controllers
{
    [TestClass()]
    public class IssuesControllerTests
    {
        [TestMethod()]
        public async Task DeleteAsyncTestSuccess()
        {
            var resultIssue = Issue.Create("Screen flickering", "It's driving me insane.", IssueStatus.Open);

            var issue = resultIssue.Value;

            Mock<IIssueStore> storeMock = new();
            storeMock
                .Setup(x => x.GetByIdAsync(123))
                .ReturnsAsync(issue);
            Mock<IUnitOfWork> unitMock = new();
            unitMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            IssueService issueService = new(storeMock.Object, unitMock.Object);

            var result = await issueService.DeleteIssueAsync(123);

            storeMock.Verify(x => x.GetByIdAsync(123), Times.Once());
            storeMock.Verify(x => x.Delete(issue), Times.Once()); // doesn't return anything, so no setup needed on the mock?
            unitMock.Verify(x => x.SaveChangesAsync(), Times.Once());
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod()]
        public async Task DeleteAsyncTestFailure()
        {
            Mock<IIssueStore> storeMock = new();
            storeMock
                .Setup(x => x.GetByIdAsync(123))
                .ReturnsAsync((Issue?)null);
            Mock<IUnitOfWork> unitMock = new();

            IssueService issueService = new(storeMock.Object, unitMock.Object);

            var result = await issueService.DeleteIssueAsync(123);

            storeMock.Verify(x => x.GetByIdAsync(123), Times.Once());
            unitMock.Verify(x => x.SaveChangesAsync(), Times.Never());
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual(ErrorType.NotFound, result.Error.Type);
        }

    }
}