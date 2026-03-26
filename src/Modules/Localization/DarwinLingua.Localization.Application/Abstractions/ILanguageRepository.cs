using DarwinLingua.Localization.Domain.Entities;

namespace DarwinLingua.Localization.Application.Abstractions;

/// <summary>
/// Defines persistence access for localization reference data.
/// </summary>
public interface ILanguageRepository
{
    /// <summary>
    /// Returns the active languages from the persistence store.
    /// </summary>
    /// <param name="cancellationToken">The token used to cancel the query.</param>
    /// <returns>The active languages.</returns>
    Task<IReadOnlyList<Language>> GetActiveAsync(CancellationToken cancellationToken);
}
