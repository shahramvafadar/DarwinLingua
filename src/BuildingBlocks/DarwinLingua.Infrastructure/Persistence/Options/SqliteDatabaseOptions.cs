namespace DarwinLingua.Infrastructure.Persistence.Options;

/// <summary>
/// Holds the SQLite database path configured by the current host.
/// </summary>
public sealed class SqliteDatabaseOptions
{
    /// <summary>
    /// Gets or sets the absolute path of the SQLite database file.
    /// </summary>
    public string DatabasePath { get; set; } = string.Empty;
}
