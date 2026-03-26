using DarwinLingua.Catalog.Domain.Entities;
using DarwinLingua.SharedKernel.Lexicon;

namespace DarwinLingua.Catalog.Application.Abstractions;

/// <summary>
/// Provides read access to lexical entry aggregates needed by Phase 1 catalog queries.
/// </summary>
public interface IWordEntryRepository
{
    /// <summary>
    /// Loads the active lexical entries linked to the specified topic key.
    /// </summary>
    Task<IReadOnlyList<WordEntry>> GetActiveByTopicKeyAsync(string topicKey, CancellationToken cancellationToken);

    /// <summary>
    /// Loads a lexical entry aggregate by its public identifier.
    /// </summary>
    Task<WordEntry?> GetByPublicIdAsync(Guid publicId, CancellationToken cancellationToken);

    /// <summary>
    /// Loads the active lexical entries for the specified CEFR level.
    /// </summary>
    Task<IReadOnlyList<WordEntry>> GetActiveByCefrAsync(CefrLevel cefrLevel, CancellationToken cancellationToken);

    /// <summary>
    /// Searches active lexical entries by normalized lemma text.
    /// </summary>
    Task<IReadOnlyList<WordEntry>> SearchActiveByLemmaAsync(string normalizedLemmaQuery, CancellationToken cancellationToken);
}
