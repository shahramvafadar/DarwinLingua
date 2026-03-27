using DarwinLingua.Practice.Application.Models;

namespace DarwinLingua.Practice.Application.Abstractions;

/// <summary>
/// Exposes learner-facing progress snapshot use cases.
/// </summary>
public interface IPracticeLearningProgressSnapshotService
{
    /// <summary>
    /// Returns a current progress snapshot built from persisted practice state and attempts.
    /// </summary>
    Task<PracticeLearningProgressSnapshotModel> GetSnapshotAsync(CancellationToken cancellationToken);
}
