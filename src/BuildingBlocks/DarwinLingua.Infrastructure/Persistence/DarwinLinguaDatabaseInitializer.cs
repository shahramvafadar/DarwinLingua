using DarwinLingua.Infrastructure.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DarwinLingua.Infrastructure.Persistence;

/// <summary>
/// Creates the local SQLite database and executes all registered seed workflows.
/// </summary>
internal sealed class DarwinLinguaDatabaseInitializer : IDatabaseInitializer
{
    private readonly IDbContextFactory<DarwinLinguaDbContext> _dbContextFactory;
    private readonly IReadOnlyCollection<IDatabaseSeeder> _databaseSeeders;

    /// <summary>
    /// Initializes a new instance of the <see cref="DarwinLinguaDatabaseInitializer"/> class.
    /// </summary>
    /// <param name="dbContextFactory">The context factory used to create database sessions.</param>
    /// <param name="databaseSeeders">The registered module seeders.</param>
    public DarwinLinguaDatabaseInitializer(
        IDbContextFactory<DarwinLinguaDbContext> dbContextFactory,
        IEnumerable<IDatabaseSeeder> databaseSeeders)
    {
        ArgumentNullException.ThrowIfNull(dbContextFactory);
        ArgumentNullException.ThrowIfNull(databaseSeeders);

        _dbContextFactory = dbContextFactory;
        _databaseSeeders = databaseSeeders.ToArray();
    }

    /// <inheritdoc />
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        await using DarwinLinguaDbContext dbContext = await _dbContextFactory
            .CreateDbContextAsync(cancellationToken)
            .ConfigureAwait(false);

        // The first implementation uses EnsureCreated so the local database becomes usable
        // immediately. Migration-based evolution can replace this once the first migrations land.
        await dbContext.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);

        foreach (IDatabaseSeeder databaseSeeder in _databaseSeeders)
        {
            await databaseSeeder.SeedAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
