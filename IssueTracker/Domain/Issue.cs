using IssueTracker.Shared;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace IssueTracker.Domain
{
    public sealed class Issue
    {
        private Issue(string title, string description, DateTimeOffset createdAt, IssueStatus status)
        {
            Title = title;
            Description = description;
            CreatedAt = createdAt;
            Status = status;
        }
         
        public int Id { get; init; }
        [MaxLength(50)] public string Title { get; private set; }
        [MaxLength(500)] public string Description { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset? UpdatedAt { get; private set; }
        public IssueStatus Status { get; private set; }
        public byte[] RowVersion { get; private set; } = [];


        public static Result<Issue> Create(string title, string description, IssueStatus status)
        {
            if (!IsValidTitle(title)) return Result<Issue>.Failure(IssueErrors.InvalidTitle);
            if (!IsValidDescription(description)) return Result<Issue>.Failure(IssueErrors.InvalidDescription);
            if (!IsValidCreateStatus(status)) return Result<Issue>.Failure(IssueErrors.InvalidCreateStatus);

            return Result<Issue>.Success(new Issue(title, description, DateTimeOffset.UtcNow, status));
        }

        public Result Update(string title, string description, IssueStatus status)
        {
            if (!IsValidTitle(title)) return Result.Failure(IssueErrors.InvalidTitle);
            if (!IsValidDescription(description)) return Result.Failure(IssueErrors.InvalidDescription);

            Title = title;
            Description = description;
            Status = status;
            UpdatedAt = DateTimeOffset.UtcNow;
            return Result.Success();
        }

        private static bool IsValidTitle(string title) => !string.IsNullOrWhiteSpace(title) && title.Length <= 50;
        private static bool IsValidDescription(string description) => !string.IsNullOrWhiteSpace(description) && description.Length <= 500;
        private static bool IsValidCreateStatus(IssueStatus status) => status != IssueStatus.Closed;
    }
}