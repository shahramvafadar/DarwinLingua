using DarwinLingua.Practice.Application.Abstractions;
using DarwinLingua.Practice.Application.Models;
using DarwinLingua.SharedKernel.Globalization;

namespace DarwinLingua.Practice.Application.Services;

/// <summary>
/// Implements learner-facing recent practice activity workflows.
/// </summary>
internal sealed class PracticeRecentActivityService : IPracticeRecentActivityService
{
    private readonly IPracticeOverviewReader _practiceOverviewReader;

    /// <summary>
    /// Initializes a new instance of the <see cref="PracticeRecentActivityService"/> class.
    /// </summary>
    public PracticeRecentActivityService(IPracticeOverviewReader practiceOverviewReader)
    {
        ArgumentNullException.ThrowIfNull(practiceOverviewReader);

        _practiceOverviewReader = practiceOverviewReader;
    }

    /// <inheritdoc />
    public Task<PracticeRecentActivityModel> GetRecentActivityAsync(
        string meaningLanguageCode,
        int maxItemCount,
        CancellationToken cancellationToken)
    {
        if (maxItemCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxItemCount), maxItemCount, "Max item count must be greater than zero.");
        }

        LanguageCode resolvedMeaningLanguageCode = LanguageCode.From(meaningLanguageCode);
        return _practiceOverviewReader.GetRecentActivityAsync(
            resolvedMeaningLanguageCode,
            maxItemCount,
            cancellationToken);
    }
}
