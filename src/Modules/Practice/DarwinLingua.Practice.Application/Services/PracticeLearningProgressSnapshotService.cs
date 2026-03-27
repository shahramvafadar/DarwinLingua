using DarwinLingua.Practice.Application.Abstractions;
using DarwinLingua.Practice.Application.Models;

namespace DarwinLingua.Practice.Application.Services;

/// <summary>
/// Implements learner-facing aggregate progress snapshot workflows.
/// </summary>
internal sealed class PracticeLearningProgressSnapshotService : IPracticeLearningProgressSnapshotService
{
    private readonly IPracticeOverviewReader _practiceOverviewReader;

    /// <summary>
    /// Initializes a new instance of the <see cref="PracticeLearningProgressSnapshotService"/> class.
    /// </summary>
    public PracticeLearningProgressSnapshotService(IPracticeOverviewReader practiceOverviewReader)
    {
        ArgumentNullException.ThrowIfNull(practiceOverviewReader);

        _practiceOverviewReader = practiceOverviewReader;
    }

    /// <inheritdoc />
    public Task<PracticeLearningProgressSnapshotModel> GetSnapshotAsync(CancellationToken cancellationToken)
    {
        return _practiceOverviewReader.GetLearningProgressSnapshotAsync(cancellationToken);
    }
}
