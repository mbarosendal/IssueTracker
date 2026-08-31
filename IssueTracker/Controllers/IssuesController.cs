using IssueTracker.Domain;
using IssueTracker.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace IssueTracker.Controllers
{
    public sealed class CreateIssueRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Title { get; init; } = string.Empty;
        [Required]
        [StringLength(500, MinimumLength = 1)]
        public string Description { get; init; } = string.Empty;
    }

    public sealed class CreateIssueResponse
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public DateTimeOffset CreatedAt { get; init; }
        public IssueStatus Status { get; init; }
    }

    public sealed class GetIssueResponse
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public DateTimeOffset CreatedAt { get; init; }
        public IssueStatus Status { get; init; }
    }

    [ApiController]
    [Route("issues")]
    public class IssuesController(IssueService service) : ControllerBase
    {

        [HttpPost()]
        public async Task<ActionResult<CreateIssueResponse>> Create(CreateIssueRequest request)
        {
            // mapping to input model for the service layer
            var input = new CreateIssueInput(
                request.Title,
                request.Description
            );

            try
            {
                var result = await service.CreateIssue(input);

                var response = new CreateIssueResponse
                {
                    Id = result.Id,
                    Title = result.Title,
                    Description = result.Description,
                    CreatedAt = result.CreatedAt,
                    Status = result.Status
                };

                // future switch expression
                return Created(
                    $"/issues/{response.Id}",
                    response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpGet()]
        public ActionResult<List<GetIssueResponse>> GetAll()
        {
            var result = service.GetAllIssues();

            var response = result.Select(issue => new GetIssueResponse
            {
                Id = issue.Id,
                Title = issue.Title,
                Description = issue.Description,
                CreatedAt = issue.CreatedAt,
                Status = issue.Status
            }).ToList();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult<GetIssueResponse> Get(int id)
        {
            var result = service.GetById(id);

            if (result is null)
            {
                return NotFound();
            }

            var response = new GetIssueResponse
            { 
                Id = result.Id,
                Title = result.Title,
                Description = result.Description,
                CreatedAt = result.CreatedAt,
                Status = result.Status
                };

            return Ok(response);
        }
    }
}
