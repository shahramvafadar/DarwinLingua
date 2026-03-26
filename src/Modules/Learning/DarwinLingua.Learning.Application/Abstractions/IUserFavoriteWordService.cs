using DarwinLingua.Learning.Application.Models;

namespace DarwinLingua.Learning.Application.Abstractions;

/// <summary>
/// Coordinates the Phase 1 favorite-word workflows.
/// </summary>
public interface IUserFavoriteWordService
{
    /// <summary>
    /// Determines whether the specified lexical entry is currently favorited.
    /// </summary>
    Task<bool> IsFavoriteAsync(Guid wordEntryPublicId, CancellationToken cancellationToken);

    /// <summary>
    /// Toggles the favorite state for the specified lexical entry and returns the resulting state.
    /// </summary>
    Task<bool> ToggleFavoriteAsync(Guid wordEntryPublicId, CancellationToken cancellationToken);

    /// <summary>
    /// Loads the current user's favorite words using the requested meaning language.
    /// </summary>
    Task<IReadOnlyList<FavoriteWordListItemModel>> GetFavoriteWordsAsync(
        string meaningLanguageCode,
        CancellationToken cancellationToken);
}
