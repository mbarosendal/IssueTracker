namespace IssueTracker.Shared
{
    public enum ErrorType
    {
        None,
        Validation,
        NotFound,
        Conflict,
        Unauthorized,
        Forbidden
    };

    public sealed record Error(string Code, string Description, ErrorType Type)
    {
        public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);
    }
}
