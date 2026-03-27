using DarwinLingua.Practice.Application.Abstractions;
using DarwinLingua.Practice.Application.Models;
using DarwinLingua.SharedKernel.Globalization;

namespace DarwinLingua.Practice.Application.Services;

/// <summary>
/// Implements learner-facing review-queue workflows.
/// </summary>
internal sealed class PracticeReviewQueueService : IPracticeReviewQueueService
{
    private readonly IPracticeOverviewReader _practiceOverviewReader;

    /// <summary>
    /// Initializes a new instance of the <see cref="PracticeReviewQueueService"/> class.
    /// </summary>
    public PracticeReviewQueueService(IPracticeOverviewReader practiceOverviewReader)
    {
        ArgumentNullException.ThrowIfNull(practiceOverviewReader);

        _practiceOverviewReader = practiceOverviewReader;
    }

    /// <inheritdoc />
    public Task<PracticeReviewQueueModel> GetQueueAsync(string meaningLanguageCode, CancellationToken cancellationToken)
    {
        LanguageCode resolvedMeaningLanguageCode = LanguageCode.From(meaningLanguageCode);
        return _practiceOverviewReader.GetReviewQueueAsync(resolvedMeaningLanguageCode, cancellationToken);
    }
}
