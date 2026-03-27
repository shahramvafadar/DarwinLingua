using DarwinLingua.Practice.Application.Abstractions;
using DarwinLingua.Practice.Application.Models;
using DarwinLingua.SharedKernel.Globalization;

namespace DarwinLingua.Practice.Application.Services;

/// <summary>
/// Implements learner-facing review-session startup workflows.
/// </summary>
internal sealed class PracticeReviewSessionService : IPracticeReviewSessionService
{
    private readonly IPracticeOverviewReader _practiceOverviewReader;

    /// <summary>
    /// Initializes a new instance of the <see cref="PracticeReviewSessionService"/> class.
    /// </summary>
    public PracticeReviewSessionService(IPracticeOverviewReader practiceOverviewReader)
    {
        ArgumentNullException.ThrowIfNull(practiceOverviewReader);

        _practiceOverviewReader = practiceOverviewReader;
    }

    /// <inheritdoc />
    public Task<PracticeReviewSessionModel> StartAsync(
        string meaningLanguageCode,
        int desiredItemCount,
        CancellationToken cancellationToken)
    {
        if (desiredItemCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(desiredItemCount), desiredItemCount, "Desired item count must be greater than zero.");
        }

        LanguageCode resolvedMeaningLanguageCode = LanguageCode.From(meaningLanguageCode);
        return _practiceOverviewReader.GetReviewSessionAsync(
            resolvedMeaningLanguageCode,
            desiredItemCount,
            cancellationToken);
    }
}
