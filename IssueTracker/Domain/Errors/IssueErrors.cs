using IssueTracker.Shared;

namespace IssueTracker.Domain.Errors
{    public static class IssueErrors
    {
        public static readonly Error InvalidTitle = new("Issue.InvalidTitle", "Title is invalid.", ErrorType.Validation);
        public static readonly Error InvalidDescription = new("Issue.InvalidDescription", "Description is invalid.", ErrorType.Validation);
        public static readonly Error InvalidCreateStatus = new("Issue.InvalidCreateStatus", "An issue cannot be created with Closed status.", ErrorType.Validation);
        public static Error NotFound(int id) => new("Issue.NotFound", $"Issue with {id} not found.", ErrorType.NotFound);
    }
}
