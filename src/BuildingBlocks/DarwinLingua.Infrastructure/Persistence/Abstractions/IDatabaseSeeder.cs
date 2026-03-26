namespace DarwinLingua.Infrastructure.Persistence.Abstractions;

/// <summary>
/// Defines a module-specific database seed step executed during startup initialization.
/// </summary>
public interface IDatabaseSeeder
{
    /// <summary>
    /// Seeds or refreshes stable reference data inside the local database.
    /// </summary>
    /// <param name="cancellationToken">The token used to cancel the seeding operation.</param>
    /// <returns>A task that completes when seeding finishes.</returns>
    Task SeedAsync(CancellationToken cancellationToken);
}
