using IssueTracker;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Respawn;
using Respawn.Graph;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace IssueTrackerTests
{
    public sealed class IntegrationTestDatabase
    {
        private readonly string _connectionString;
        private readonly Respawner _respawner;

        private IntegrationTestDatabase(
            string connectionString,
            Respawner respawner)
        {
            _connectionString = connectionString;
            _respawner = respawner;
        }

        public static async Task<IntegrationTestDatabase> CreateAsync(
            string connectionString)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            await using var context = new AppDbContext(options);

            await context.Database.MigrateAsync();

            var connection = context.Database.GetDbConnection();

            await connection.OpenAsync();

            var respawner = await Respawner.CreateAsync(
                connection,
                new RespawnerOptions
                {
                    TablesToIgnore = new Table[]
                    {
                        "__EFMigrationsHistory"
                    }
                });

            return new IntegrationTestDatabase(
                connectionString,
                respawner);
        }

        public async Task ResetAsync()
        {
            await using var connection = new SqlConnection(_connectionString);

            await connection.OpenAsync();

            await _respawner.ResetAsync(connection);
        }

        public async Task<AppDbContext> CreateContextAsync()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(_connectionString)
                .Options;

            return new AppDbContext(options);
        }
    }
}
