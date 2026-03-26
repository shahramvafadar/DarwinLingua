namespace DarwinLingua.SharedKernel.Abstractions.Time;

/// <summary>
/// Represents a time provider used by application and infrastructure code.
/// </summary>
public interface IClock
{
    /// <summary>
    /// Gets the current UTC timestamp.
    /// </summary>
    DateTimeOffset UtcNow { get; }
}
