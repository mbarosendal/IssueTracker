using IssueTracker.Domain;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
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
