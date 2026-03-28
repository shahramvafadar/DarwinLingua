namespace DarwinDeutsch.Maui.Services.Storage;

/// <summary>
/// Provides first-run database provisioning and packaged-seed update application.
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

    /// <summary>
    /// Gets the current packaged-seed update status for the local database.
    /// </summary>
    /// <param name="databasePath">The local sandbox database path used by the app.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The update status for the packaged seed.</returns>
    Task<SeedDatabaseUpdateStatus> GetUpdateStatusAsync(string databasePath, CancellationToken cancellationToken);

    /// <summary>
    /// Applies new packaged content from the bundled seed database into the local sandbox database.
    /// </summary>
    /// <param name="databasePath">The local sandbox database path used by the app.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the packaged-seed update operation.</returns>
    Task<SeedDatabaseUpdateResult> ApplySeedUpdateAsync(string databasePath, CancellationToken cancellationToken);
}
