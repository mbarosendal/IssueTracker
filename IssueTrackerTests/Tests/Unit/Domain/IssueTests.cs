using IssueTracker.Domain;
using IssueTracker.Domain.Errors;
using IssueTracker.Shared;

namespace IssueTrackerTests.Tests.Unit.Domain
{
    [TestClass]
    public class IssueTests
    {
        [TestMethod]
        public void Create_WithClosedStatus_ReturnsValidationError()
        {
            // Act
            var issueResult = Issue.Create("testTitle", "testDescription", IssueStatus.Closed);

            // Assert
            Assert.IsFalse(issueResult.IsSuccess);
            Assert.AreEqual(ErrorType.Validation, issueResult.Error.Type);
            Assert.AreEqual(IssueErrors.InvalidCreateStatus, issueResult.Error);
        }
    }
}
