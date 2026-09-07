using IssueTracker.Domain.Shared;

namespace IssueTracker.Domain
{    public static class IssueErrors
    {
        public static readonly Error InvalidTitle = new("Issue.InvalidTitle", "Title is invalid.");
        public static readonly Error InvalidDescription = new("Issue.InvalidDescription", "Description is invalid.");
    }
}
