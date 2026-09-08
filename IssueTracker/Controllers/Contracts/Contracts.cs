using IssueTracker.Domain;
using System.ComponentModel.DataAnnotations;

namespace IssueTracker.Controllers.Contracts
{
    public class Contracts
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

    }
}
