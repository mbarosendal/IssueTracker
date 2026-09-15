using IssueTracker.Domain;
using System.ComponentModel.DataAnnotations;

namespace IssueTracker.Controllers.Contracts
{
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
