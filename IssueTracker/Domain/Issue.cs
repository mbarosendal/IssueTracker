using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace IssueTracker.Domain
{
    public abstract record Result
    {
        public sealed record Success : Result;

        public sealed record InvalidTitle : Result;

        public sealed record InvalidDescription : Result;
    }

    public abstract record Result<T>
    {
        public sealed record Success(T Value) : Result<T>;

        public sealed record Failure(Result Error) : Result<T>;
    }
    public sealed class Issue
    {
        private Issue(
            string title,
            string description,
            DateTimeOffset createdAt,
            IssueStatus status)
        {
            Title = title;
            Description = description;
            CreatedAt = createdAt;
            Status = status;
        }

        public int Id { get; init; }

        [MaxLength(50)]
        public string Title { get; private set; }

        [MaxLength(500)]
        public string Description { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }

        public DateTimeOffset? UpdatedAt { get; private set; }

        public IssueStatus Status { get; private set; }

        public static Result<Issue> Create(
            string title,
            string description,
            IssueStatus status)
        {
            if (!IsValidTitle(title))
                return new Result<Issue>.Failure(new Result.InvalidTitle());

            if (!IsValidDescription(description))
                return new Result<Issue>.Failure(new Result.InvalidDescription());

            var issue = new Issue(
                title,
                description,
                DateTimeOffset.UtcNow,
                status);

            return new Result<Issue>.Success(issue);
        }

        public Result Update(
            string title,
            string description,
            IssueStatus status)
        {
            if (!IsValidTitle(title))
                return new Result.InvalidTitle();

            if (!IsValidDescription(description))
                return new Result.InvalidDescription();

            Title = title;
            Description = description;
            Status = status;
            UpdatedAt = DateTimeOffset.UtcNow;

            return new Result.Success();
        }

        private static bool IsValidTitle(string title) =>
            !string.IsNullOrWhiteSpace(title) &&
            title.Length <= 50;

        private static bool IsValidDescription(string description) =>
            !string.IsNullOrWhiteSpace(description) &&
            description.Length <= 500;
    }
}
