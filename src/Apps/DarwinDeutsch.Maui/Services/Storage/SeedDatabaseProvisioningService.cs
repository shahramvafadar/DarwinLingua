namespace DarwinDeutsch.Maui.Services.Storage;

/// <summary>
/// Provides the initial local SQLite database by copying the packaged seed database asset on first launch.
/// </summary>
internal sealed class SeedDatabaseProvisioningService : ISeedDatabaseProvisioningService
{
    private const string SeedDatabaseAssetName = "darwin-lingua.seed.db";

    /// <inheritdoc />
    public async Task EnsureSeedDatabaseAsync(string databasePath, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(databasePath);

        if (File.Exists(databasePath))
        {
            return;
        }

        string? databaseDirectory = Path.GetDirectoryName(databasePath);
        if (!string.IsNullOrWhiteSpace(databaseDirectory))
        {
            Directory.CreateDirectory(databaseDirectory);
        }

        try
        {
            await using Stream packagedSeedStream = await FileSystem.Current
                .OpenAppPackageFileAsync(SeedDatabaseAssetName)
                .WaitAsync(cancellationToken)
                .ConfigureAwait(false);

            await using FileStream localDatabaseStream = File.Create(databasePath);
            await packagedSeedStream.CopyToAsync(localDatabaseStream, cancellationToken).ConfigureAwait(false);
        }
        catch (FileNotFoundException)
        {
            // Fall back to normal schema creation when the packaged seed database is not present.
        }
    }
}
