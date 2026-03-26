using DarwinLingua.Catalog.Application.Models;

namespace DarwinLingua.Catalog.Application.Abstractions;

/// <summary>
/// Provides read-only lexical queries for browse scenarios.
/// </summary>
public interface IWordQueryService
{
    /// <summary>
    /// Returns active lexical entries linked to the specified topic key.
    /// </summary>
    Task<IReadOnlyList<WordListItemModel>> GetWordsByTopicAsync(
        string topicKey,
        string meaningLanguageCode,
        CancellationToken cancellationToken);

    /// <summary>
    /// Returns active lexical entries for the specified CEFR level.
    /// </summary>
    Task<IReadOnlyList<WordListItemModel>> GetWordsByCefrAsync(
        string cefrLevel,
        string meaningLanguageCode,
        CancellationToken cancellationToken);

    /// <summary>
    /// Searches active lexical entries by German lemma text.
    /// </summary>
    Task<IReadOnlyList<WordListItemModel>> SearchWordsAsync(
        string query,
        string meaningLanguageCode,
        CancellationToken cancellationToken);
}
