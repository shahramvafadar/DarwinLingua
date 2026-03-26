using DarwinLingua.Catalog.Application.Abstractions;
using DarwinLingua.Catalog.Application.DependencyInjection;
using DarwinLingua.Catalog.Application.Models;
using DarwinLingua.Catalog.Domain.Entities;
using DarwinLingua.SharedKernel.Content;
using DarwinLingua.SharedKernel.Globalization;
using DarwinLingua.SharedKernel.Lexicon;
using Microsoft.Extensions.DependencyInjection;

namespace DarwinLingua.Catalog.Application.Tests;

/// <summary>
/// Verifies the lexical browse query behavior.
/// </summary>
public sealed class WordQueryServiceTests
{
    /// <summary>
    /// Verifies that the query service selects the requested meaning language when available.
    /// </summary>
    [Fact]
    public async Task GetWordsByTopicAsync_ShouldReturnRequestedMeaningLanguage()
    {
        WordEntry word = new(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Bahnhof",
            LanguageCode.From("de"),
            CefrLevel.A1,
            PartOfSpeech.Noun,
            PublicationStatus.Active,
            ContentSourceType.Manual,
            DateTime.UtcNow,
            article: "der");

        WordSense sense = word.AddSense(Guid.NewGuid(), 1, true, PublicationStatus.Active, DateTime.UtcNow);
        sense.AddTranslation(Guid.NewGuid(), LanguageCode.From("en"), "station", true, DateTime.UtcNow);
        sense.AddTranslation(Guid.NewGuid(), LanguageCode.From("fa"), "ایستگاه", false, DateTime.UtcNow);
        word.AddTopic(Guid.NewGuid(), Guid.NewGuid(), true, DateTime.UtcNow);

        ServiceCollection services = new();
        services.AddCatalogApplication();
        services.AddSingleton<IWordEntryRepository>(new FakeWordEntryRepository([word]));

        await using ServiceProvider serviceProvider = services.BuildServiceProvider();
        IWordQueryService queryService = serviceProvider.GetRequiredService<IWordQueryService>();

        IReadOnlyList<WordListItemModel> words = await queryService
            .GetWordsByTopicAsync("travel", "fa", CancellationToken.None);

        WordListItemModel result = Assert.Single(words);
        Assert.Equal("Bahnhof", result.Lemma);
        Assert.Equal("ایستگاه", result.PrimaryMeaning);
    }

    /// <summary>
    /// Verifies that the CEFR query returns only matching words.
    /// </summary>
    [Fact]
    public async Task GetWordsByCefrAsync_ShouldReturnMatchingWords()
    {
        WordEntry a1Word = CreateWord("Bahnhof", CefrLevel.A1, "station");
        WordEntry b1Word = CreateWord("Bewerbung", CefrLevel.B1, "application");

        ServiceCollection services = new();
        services.AddCatalogApplication();
        services.AddSingleton<IWordEntryRepository>(new FakeWordEntryRepository([a1Word, b1Word]));

        await using ServiceProvider serviceProvider = services.BuildServiceProvider();
        IWordQueryService queryService = serviceProvider.GetRequiredService<IWordQueryService>();

        IReadOnlyList<WordListItemModel> words = await queryService
            .GetWordsByCefrAsync("A1", "en", CancellationToken.None);

        WordListItemModel result = Assert.Single(words);
        Assert.Equal("Bahnhof", result.Lemma);
    }

    /// <summary>
    /// Verifies that search normalizes whitespace and casing.
    /// </summary>
    [Fact]
    public async Task SearchWordsAsync_ShouldNormalizeSearchQuery()
    {
        WordEntry word = CreateWord("Bahnhof", CefrLevel.A1, "station");

        ServiceCollection services = new();
        services.AddCatalogApplication();
        services.AddSingleton<IWordEntryRepository>(new FakeWordEntryRepository([word]));

        await using ServiceProvider serviceProvider = services.BuildServiceProvider();
        IWordQueryService queryService = serviceProvider.GetRequiredService<IWordQueryService>();

        IReadOnlyList<WordListItemModel> words = await queryService
            .SearchWordsAsync("  BAHNHOF  ", "en", CancellationToken.None);

        WordListItemModel result = Assert.Single(words);
        Assert.Equal("Bahnhof", result.Lemma);
    }

    private sealed class FakeWordEntryRepository(IReadOnlyList<WordEntry> words) : IWordEntryRepository
    {
        public Task<IReadOnlyList<WordEntry>> GetActiveByTopicKeyAsync(string topicKey, CancellationToken cancellationToken)
        {
            return Task.FromResult(words);
        }

        public Task<WordEntry?> GetByPublicIdAsync(Guid publicId, CancellationToken cancellationToken)
        {
            return Task.FromResult(words.SingleOrDefault(word => word.PublicId == publicId));
        }

        public Task<IReadOnlyList<WordEntry>> GetActiveByCefrAsync(CefrLevel cefrLevel, CancellationToken cancellationToken)
        {
            return Task.FromResult<IReadOnlyList<WordEntry>>(words.Where(word => word.PrimaryCefrLevel == cefrLevel).ToArray());
        }

        public Task<IReadOnlyList<WordEntry>> SearchActiveByLemmaAsync(string normalizedLemmaQuery, CancellationToken cancellationToken)
        {
            return Task.FromResult<IReadOnlyList<WordEntry>>(words
                .Where(word => word.NormalizedLemma.Contains(normalizedLemmaQuery, StringComparison.Ordinal))
                .ToArray());
        }
    }

    private static WordEntry CreateWord(string lemma, CefrLevel cefrLevel, string translationText)
    {
        WordEntry word = new(
            Guid.NewGuid(),
            Guid.NewGuid(),
            lemma,
            LanguageCode.From("de"),
            cefrLevel,
            PartOfSpeech.Noun,
            PublicationStatus.Active,
            ContentSourceType.Manual,
            DateTime.UtcNow,
            article: "der");
        WordSense sense = word.AddSense(Guid.NewGuid(), 1, true, PublicationStatus.Active, DateTime.UtcNow);
        sense.AddTranslation(Guid.NewGuid(), LanguageCode.From("en"), translationText, true, DateTime.UtcNow);
        return word;
    }
}
