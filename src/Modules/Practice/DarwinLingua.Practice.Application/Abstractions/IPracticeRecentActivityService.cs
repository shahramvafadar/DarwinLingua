using DarwinLingua.Practice.Application.Models;

namespace DarwinLingua.Practice.Application.Abstractions;

/// <summary>
/// Exposes learner-facing recent-practice activity use cases.
/// </summary>
public interface IPracticeRecentActivityService
{
    /// <summary>
    /// Returns the most recent persisted practice attempts for the requested meaning language.
    /// </summary>
    Task<PracticeRecentActivityModel> GetRecentActivityAsync(
        string meaningLanguageCode,
        int maxItemCount,
        CancellationToken cancellationToken);
}
