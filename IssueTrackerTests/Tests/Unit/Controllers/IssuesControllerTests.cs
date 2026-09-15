using IssueTracker.Controllers;
using IssueTracker.Controllers.Contracts;
using IssueTracker.Domain;
using IssueTracker.Services;
using IssueTracker.Services.Contracts;
using IssueTracker.Services.Errors;
using IssueTracker.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace IssueTrackerTests.Tests.Unit.Controllers
{
    [TestClass]
    public class IssuesControllerTests
    {
        [TestMethod]
        public async Task UpdateAsync_WhenConcurrencyConflictOccurs_ReturnsConflict()
        {
            // Arrange
            var concurrencyError = IssueServiceErrors.ConcurrencyConflict;

            var issueServiceMock = new Mock<IIssueService>();
            issueServiceMock
                .Setup(x => x.UpdateIssueAsync(
                    123,
                    It.IsAny<UpdateIssueInput>()))
                .ReturnsAsync(Result<UpdateIssueOutput>.Failure(concurrencyError));

            var controller = new IssuesController(issueServiceMock.Object);

            var request = new UpdateIssueRequest
            {
                Title = "updatedTitle",
                Description = "updatedDescription",
                Status = IssueStatus.InProgress
            };

            // Act
            var result = await controller.UpdateAsync(123, request);

            // Assert
            var objectResult = result.Result as ObjectResult;

            Assert.IsNotNull(objectResult);
            Assert.AreEqual(StatusCodes.Status409Conflict, objectResult.StatusCode);

            var problemDetails = objectResult.Value as ProblemDetails;

            Assert.IsNotNull(problemDetails);
            Assert.AreEqual(concurrencyError.Code, problemDetails.Title);
            Assert.AreEqual(concurrencyError.Description, problemDetails.Detail);
        }
    }
}
