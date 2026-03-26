using DarwinLingua.Localization.Application.Models;

namespace DarwinLingua.Localization.Application.Abstractions;

/// <summary>
/// Exposes read-only localization language queries used by the presentation layer.
/// </summary>
public interface ILanguageQueryService
{
    /// <summary>
    /// Returns the active languages currently available in the local database.
    /// </summary>
    /// <param name="cancellationToken">The token used to cancel the query.</param>
    /// <returns>The list of active languages ordered for display.</returns>
    Task<IReadOnlyList<SupportedLanguageModel>> GetActiveLanguagesAsync(CancellationToken cancellationToken);
}
