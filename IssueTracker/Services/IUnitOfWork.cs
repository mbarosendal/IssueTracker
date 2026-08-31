namespace IssueTracker.Services
{
    public interface IUnitOfWork

    {
        Task SaveChangesAsync();
    }
}
