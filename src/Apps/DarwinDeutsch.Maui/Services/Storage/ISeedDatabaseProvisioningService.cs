namespace DarwinDeutsch.Maui.Services.Storage;

/// <summary>
/// Copies the packaged SQLite seed database into the app sandbox when the local database is missing.
/// </summary>
public interface ISeedDatabaseProvisioningService
{
    /// <summary>
    /// Ensures that the local database file exists, using the packaged seed database when available.
    /// </summary>
    /// <param name="databasePath">The local sandbox database path used by the app.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that completes when the database file has been provisioned if needed.</returns>
    Task EnsureSeedDatabaseAsync(string databasePath, CancellationToken cancellationToken);
}
