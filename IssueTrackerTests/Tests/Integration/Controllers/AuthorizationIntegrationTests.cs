using IssueTrackerTests.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace IssueTrackerTests.Tests.Integration.Controllers
{
    [TestClass]
    public class AuthorizationIntegrationTests : DatabaseFixture
    {
        private static ApiFactory _factory = null!;
        private static HttpClient _client = null!;
        protected static string ConnectionString => _connectionString;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            _factory = new ApiFactory(_connectionString);
            _client = _factory.CreateClient();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        [TestMethod]
        public async Task GetIssues_WhenUnauthenticated_ReturnsUnauthorized()
        {
            // Act
            var response = await _client.GetAsync("/issues");

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        [TestMethod]
        public async Task GetIssues_WhenAuthenticatedWithoutRequiredScope_ReturnsForbidden()
        {
            // Arrange
            var token = TestJwtFactory.CreateToken("issues.write");

            using var request = new HttpRequestMessage(
                HttpMethod.Get,
                "/issues");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public async Task GetIssues_WhenAuthenticatedWithRequiredScope_ReturnsOk()
        {
            // Arrange
            var token = TestJwtFactory.CreateToken("issues.read");

            using var request = new HttpRequestMessage(
                HttpMethod.Get,
                "/issues");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}

