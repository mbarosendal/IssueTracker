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
    public class IssuesController(IssueService service) : ControllerBase
    {

        [HttpPost()]
        public async Task<ActionResult<CreateIssueResponse>> CreateAsync(CreateIssueRequest request)
        {
            // mapping to input model for the service layer
            var input = new CreateIssueInput(
                request.Title,
                request.Description
            );

            var result = await service.CreateIssueAsync(input);

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

        [HttpGet()]
        public async Task<ActionResult<List<GetIssueResponse>>> GetAllAsync()
        {
            var result = await service.GetAllIssuesAsync();

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
            var result = await service.GetByIdAsync(id);

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

        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateIssueResponse>> UpdateAsync(int id, UpdateIssueRequest request)
        {
            var input = new UpdateIssueInput(
                request.Title,
                request.Description,
                request.Status
            );
            var result = await service.UpdateIssueAsync(id, input);

            if (result is null)
            {
                return NotFound();
            }

            var response = new UpdateIssueResponse
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
    }
}
