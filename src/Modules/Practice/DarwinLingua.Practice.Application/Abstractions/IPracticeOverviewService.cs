using DarwinLingua.Practice.Application.Models;

namespace DarwinLingua.Practice.Application.Abstractions;

/// <summary>
/// Exposes learner-facing practice overview use cases.
/// </summary>
public interface IPracticeOverviewService
{
    /// <summary>
    /// Returns the current local practice overview for the requested meaning language.
    /// </summary>
    Task<PracticeOverviewModel> GetOverviewAsync(string meaningLanguageCode, CancellationToken cancellationToken);
}
