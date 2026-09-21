using IssueTracker;
using Microsoft.EntityFrameworkCore;
using Respawn;
using Respawn.Graph;
using Testcontainers.MsSql;

namespace IssueTrackerTests.Shared;

public abstract class DatabaseFixture
{
    private static MsSqlContainer _container = null!;
    private static Respawner _respawner = null!;
    protected static string _connectionString = null!;

    [ClassInitialize(InheritanceBehavior.BeforeEachDerivedClass)]
    public static async Task Initialize(TestContext context)
    {
        _container = new MsSqlBuilder()
            .Build();

        await _container.StartAsync();

        _connectionString = _container.GetConnectionString();

        await using var dbContext = CreateContext();

        await dbContext.Database.MigrateAsync();

        await using var connection = dbContext.Database.GetDbConnection();

        await connection.OpenAsync();

        _respawner = await Respawner.CreateAsync(
            connection,
            new RespawnerOptions
            {
                TablesToIgnore =
                [
                    "__EFMigrationsHistory"
                ]
            });
    }

    [TestInitialize]
    public async Task ResetDatabase()
    {
        await using var connection =
            new Microsoft.Data.SqlClient.SqlConnection(_connectionString);

        await connection.OpenAsync();

        await _respawner.ResetAsync(connection);
    }

    [ClassCleanup(
        InheritanceBehavior.BeforeEachDerivedClass,
        ClassCleanupBehavior.EndOfClass)]
    public static async Task Cleanup()
    {
        await _container.DisposeAsync();
    }

    protected static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(_connectionString)
            .Options;

        return new AppDbContext(options);
    }
}