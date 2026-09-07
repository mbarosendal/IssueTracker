using IssueTracker.Domain;
using IssueTracker.Domain.Shared;
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
        public DateTimeOffset? UpdatedAt { get; init; }
        public IssueStatus Status { get; init; }
    }

    public sealed class UpdateIssueResponse
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset? UpdatedAt { get; init; }
        public IssueStatus Status { get; init; }
    }

    public sealed class UpdateIssueRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Title { get; init; } = string.Empty;
        [Required]
        [StringLength(500, MinimumLength = 1)]
        public string Description { get; init; } = string.Empty;
        public IssueStatus Status { get; init; }
    }

    [ApiController]
    [Route("issues")]
    [Produces("application/json")]
    public class IssuesController(IssueService issueService) : ControllerBase
    {
        [HttpGet("/problems")]
        public ActionResult<string> FailureTest()
        {
            throw new Exception("This is a test exception to demonstrate the global exception handling middleware.");
        }

        [HttpPost()]
        public async Task<ActionResult<CreateIssueResponse>> CreateAsync(CreateIssueRequest request)
        {
            var input = new CreateIssueInput(request.Title, request.Description);

            var result = await issueService.CreateIssueAsync(input);

            // as-value: no need to know WHICH error, just relay Code/Description to the client
            if (result.IsFailure)
            {
                return Problem(
                    title: result.Error.Code,
                    detail: result.Error.Description,
                    statusCode: StatusCodes.Status400BadRequest);
            }

            var issue = result.Value; // Value is the success object; only safe to read .Value because IsFailure was already checked above

            var response = new CreateIssueResponse
            {
                Id = issue.Id,
                Title = issue.Title,
                Description = issue.Description,
                CreatedAt = issue.CreatedAt,
                Status = issue.Status
            };

            return Created($"/issues/{response.Id}", response);
        }

        [HttpGet()]
        public async Task<ActionResult<List<GetIssueResponse>>> GetAllAsync()
        {
            // no Result here — a list query has no business-rule failure mode, just data
            var result = await issueService.GetAllIssuesAsync();

            var response = result.Select(issue => new GetIssueResponse
            {
                Id = issue.Id,
                Title = issue.Title,
                Description = issue.Description,
                CreatedAt = issue.CreatedAt,
                UpdatedAt = issue.UpdatedAt,
                Status = issue.Status
            }).ToList();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetIssueResponse>> GetAsync(int id)
        {
            // still plain nullable — "not found" here is absence of data, not a Result-worthy business failure
            var result = await issueService.GetByIdAsync(id);

            if (result is null)
                return NotFound();

            var response = new GetIssueResponse
            {
                Id = result.Id,
                Title = result.Title,
                Description = result.Description,
                CreatedAt = result.CreatedAt,
                UpdatedAt = result.UpdatedAt,
                Status = result.Status
            };

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateIssueResponse>> UpdateAsync(int id, UpdateIssueRequest request)
        {
            var input = new UpdateIssueInput(request.Title, request.Description, request.Status);

            var result = await issueService.UpdateIssueAsync(id, input);

            // null = issue didn't exist at all -> 404
            if (result is null)
                return NotFound();

            // non-null but failed = issue exists, update was invalid -> 400
            if (result.IsFailure)
            {
                return Problem(
                    title: result.Error.Code,
                    detail: result.Error.Description,
                    statusCode: StatusCodes.Status400BadRequest);
            }

            var issue = result.Value;

            var response = new UpdateIssueResponse
            {
                Id = issue.Id,
                Title = issue.Title,
                Description = issue.Description,
                CreatedAt = issue.CreatedAt,
                UpdatedAt = issue.UpdatedAt,
                Status = issue.Status
            };

            return Ok(response);
        }
    }
}