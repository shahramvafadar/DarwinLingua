namespace DarwinLingua.Infrastructure.Persistence.Abstractions;

/// <summary>
/// Defines the startup workflow responsible for creating and preparing the local database.
/// </summary>
public interface IDatabaseInitializer
{
    /// <summary>
    /// Ensures that the local database exists and all registered seeders have run.
    /// </summary>
    /// <param name="cancellationToken">The token used to cancel the initialization flow.</param>
    /// <returns>A task that completes when initialization finishes.</returns>
    Task InitializeAsync(CancellationToken cancellationToken);
}
