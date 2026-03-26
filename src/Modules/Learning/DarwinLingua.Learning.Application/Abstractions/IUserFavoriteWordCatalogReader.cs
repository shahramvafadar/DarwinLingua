using DarwinLingua.Learning.Application.Models;

namespace DarwinLingua.Learning.Application.Abstractions;

/// <summary>
/// Reads lexical data needed by the learning module for favorite-word workflows.
/// </summary>
public interface IUserFavoriteWordCatalogReader
{
    /// <summary>
    /// Determines whether an active lexical entry exists for the specified public identifier.
    /// </summary>
    Task<bool> ExistsAsync(Guid wordEntryPublicId, CancellationToken cancellationToken);

    /// <summary>
    /// Loads the current favorite words for the specified user in the requested meaning language.
    /// </summary>
    Task<IReadOnlyList<FavoriteWordListItemModel>> GetFavoriteWordsAsync(
        string userId,
        string meaningLanguageCode,
        CancellationToken cancellationToken);
}
