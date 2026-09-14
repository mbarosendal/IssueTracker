using IssueTracker.Domain;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Issue> Issues { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{

        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Issue>().Property(i => i.RowVersion).IsRowVersion();
        }
    }
}
