using DarwinLingua.Practice.Application.Models;
using DarwinLingua.SharedKernel.Globalization;

namespace DarwinLingua.Practice.Application.Abstractions;

/// <summary>
/// Reads local practice overview and review-queue data from the persistence boundary.
/// </summary>
public interface IPracticeOverviewReader
{
    /// <summary>
    /// Returns the current local practice overview for the requested meaning language.
    /// </summary>
    Task<PracticeOverviewModel> GetOverviewAsync(LanguageCode meaningLanguageCode, CancellationToken cancellationToken);

    /// <summary>
    /// Returns the current ordered review queue for the requested meaning language.
    /// </summary>
    Task<PracticeReviewQueueModel> GetReviewQueueAsync(LanguageCode meaningLanguageCode, CancellationToken cancellationToken);
}
