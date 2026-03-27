using DarwinLingua.Practice.Application.Models;

namespace DarwinLingua.Practice.Application.Abstractions;

/// <summary>
/// Exposes learner-facing review-session startup use cases.
/// </summary>
public interface IPracticeReviewSessionService
{
    /// <summary>
    /// Starts a review session snapshot from the current ordered review queue.
    /// </summary>
    Task<PracticeReviewSessionModel> StartAsync(
        string meaningLanguageCode,
        int desiredItemCount,
        CancellationToken cancellationToken);
}
