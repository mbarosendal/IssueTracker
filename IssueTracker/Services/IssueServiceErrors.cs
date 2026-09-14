using IssueTracker.Shared;

namespace IssueTracker.Services
{
    public static class IssueServiceErrors
    {
        public static readonly Error ConcurrencyConflict = new(
             "Issue.ConcurrencyConflict",
             "The issue was modified by another request. Please reload it and try again.",
             ErrorType.Conflict
            );
    }
}
