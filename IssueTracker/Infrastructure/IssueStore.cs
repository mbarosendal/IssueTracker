using IssueTracker.Domain;

namespace IssueTracker.Infrastructure
{
    public class IssueStore
    {
        private readonly List<Issue> _issues = new List<Issue>();

        public void Add(Issue issue)
        {
            _issues.Add(issue);
        }

        public List<Issue> GetAll()
        {
            return _issues.ToList();
        }

        public Issue? GetById(int id)
        {
            return _issues.FirstOrDefault(i => i.Id == id);
        }
    }
}
