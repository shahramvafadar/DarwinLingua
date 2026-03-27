using DarwinLingua.Practice.Application.Models;

namespace DarwinLingua.Practice.Application.Abstractions;

/// <summary>
/// Exposes learner-facing review-queue use cases.
/// </summary>
public interface IPracticeReviewQueueService
{
    /// <summary>
    /// Returns the current ordered review queue for the requested meaning language.
    /// </summary>
    Task<PracticeReviewQueueModel> GetQueueAsync(string meaningLanguageCode, CancellationToken cancellationToken);
}
