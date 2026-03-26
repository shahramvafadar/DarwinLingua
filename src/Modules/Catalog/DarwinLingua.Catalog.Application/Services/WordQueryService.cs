using DarwinLingua.Catalog.Application.Abstractions;
using DarwinLingua.Catalog.Application.Models;
using DarwinLingua.Catalog.Domain.Entities;
using DarwinLingua.SharedKernel.Globalization;
using DarwinLingua.SharedKernel.Lexicon;
using System.Text.RegularExpressions;

namespace DarwinLingua.Catalog.Application.Services;

/// <summary>
/// Implements browse-oriented lexical queries for the catalog module.
/// </summary>
internal sealed partial class WordQueryService : IWordQueryService
{
    private readonly IWordEntryRepository _wordEntryRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="WordQueryService"/> class.
    /// </summary>
    public WordQueryService(IWordEntryRepository wordEntryRepository)
    {
        ArgumentNullException.ThrowIfNull(wordEntryRepository);

        _wordEntryRepository = wordEntryRepository;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<WordListItemModel>> GetWordsByTopicAsync(
        string topicKey,
        string meaningLanguageCode,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(topicKey);
        LanguageCode resolvedMeaningLanguageCode = LanguageCode.From(meaningLanguageCode);

        IReadOnlyList<WordEntry> words = await _wordEntryRepository
            .GetActiveByTopicKeyAsync(topicKey, cancellationToken)
            .ConfigureAwait(false);

        return words
            .OrderBy(word => word.NormalizedLemma)
            .Select(word => Map(word, resolvedMeaningLanguageCode))
            .ToArray();
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<WordListItemModel>> GetWordsByCefrAsync(
        string cefrLevel,
        string meaningLanguageCode,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cefrLevel);

        LanguageCode resolvedMeaningLanguageCode = LanguageCode.From(meaningLanguageCode);
        CefrLevel resolvedCefrLevel = Enum.Parse<CefrLevel>(cefrLevel.Trim(), ignoreCase: true);

        IReadOnlyList<WordEntry> words = await _wordEntryRepository
            .GetActiveByCefrAsync(resolvedCefrLevel, cancellationToken)
            .ConfigureAwait(false);

        return words
            .OrderBy(word => word.NormalizedLemma)
            .Select(word => Map(word, resolvedMeaningLanguageCode))
            .ToArray();
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<WordListItemModel>> SearchWordsAsync(
        string query,
        string meaningLanguageCode,
        CancellationToken cancellationToken)
    {
        LanguageCode resolvedMeaningLanguageCode = LanguageCode.From(meaningLanguageCode);

        if (string.IsNullOrWhiteSpace(query))
        {
            return [];
        }

        string normalizedQuery = NormalizeSearchQuery(query);

        if (string.IsNullOrWhiteSpace(normalizedQuery))
        {
            return [];
        }

        IReadOnlyList<WordEntry> words = await _wordEntryRepository
            .SearchActiveByLemmaAsync(normalizedQuery, cancellationToken)
            .ConfigureAwait(false);

        return words
            .OrderBy(word => word.NormalizedLemma)
            .Select(word => Map(word, resolvedMeaningLanguageCode))
            .ToArray();
    }

    /// <summary>
    /// Maps the aggregate to a browse-friendly list item model.
    /// </summary>
    private static WordListItemModel Map(WordEntry word, LanguageCode meaningLanguageCode)
    {
        ArgumentNullException.ThrowIfNull(word);

        WordSense? primarySense = word.GetPrimarySense();
        SenseTranslation? preferredTranslation = primarySense?.Translations
            .Where(translation => translation.LanguageCode == meaningLanguageCode)
            .OrderByDescending(translation => translation.IsPrimary)
            .ThenBy(translation => translation.TranslationText)
            .FirstOrDefault();

        return new WordListItemModel(
            word.PublicId,
            word.Lemma,
            word.Article,
            word.PluralForm,
            word.PartOfSpeech.ToString(),
            word.PrimaryCefrLevel.ToString(),
            preferredTranslation?.TranslationText);
    }

    /// <summary>
    /// Normalizes the search query using the same whitespace and casing rules as lexical lemmas.
    /// </summary>
    private static string NormalizeSearchQuery(string query)
    {
        return CollapseWhitespace().Replace(query.Trim(), " ").ToLowerInvariant();
    }

    [GeneratedRegex("\\s+", RegexOptions.Compiled)]
    private static partial Regex CollapseWhitespace();
}
