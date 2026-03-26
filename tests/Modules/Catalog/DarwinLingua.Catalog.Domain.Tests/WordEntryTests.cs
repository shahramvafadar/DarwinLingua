using DarwinLingua.Catalog.Domain.Entities;
using DarwinLingua.SharedKernel.Content;
using DarwinLingua.SharedKernel.Exceptions;
using DarwinLingua.SharedKernel.Globalization;
using DarwinLingua.SharedKernel.Lexicon;

namespace DarwinLingua.Catalog.Domain.Tests;

/// <summary>
/// Verifies the <see cref="WordEntry"/> aggregate behavior.
/// </summary>
public sealed class WordEntryTests
{
    /// <summary>
    /// Verifies that duplicate sense orders are rejected.
    /// </summary>
    [Fact]
    public void AddSense_ShouldRejectDuplicateSenseOrder()
    {
        WordEntry word = CreateWordEntry();
        word.AddSense(Guid.NewGuid(), 1, true, PublicationStatus.Active, DateTime.UtcNow);

        Assert.Throws<DomainRuleException>(() => word.AddSense(
            Guid.NewGuid(),
            1,
            false,
            PublicationStatus.Active,
            DateTime.UtcNow));
    }

    /// <summary>
    /// Verifies that duplicate topic links are rejected.
    /// </summary>
    [Fact]
    public void AddTopic_ShouldRejectDuplicateTopic()
    {
        WordEntry word = CreateWordEntry();
        Guid topicId = Guid.NewGuid();
        word.AddTopic(Guid.NewGuid(), topicId, true, DateTime.UtcNow);

        Assert.Throws<DomainRuleException>(() => word.AddTopic(
            Guid.NewGuid(),
            topicId,
            false,
            DateTime.UtcNow));
    }

    /// <summary>
    /// Verifies that duplicate translation languages are rejected within a single sense.
    /// </summary>
    [Fact]
    public void AddTranslation_ShouldRejectDuplicateLanguagePerSense()
    {
        WordEntry word = CreateWordEntry();
        WordSense sense = word.AddSense(Guid.NewGuid(), 1, true, PublicationStatus.Active, DateTime.UtcNow);
        sense.AddTranslation(Guid.NewGuid(), LanguageCode.From("en"), "station", true, DateTime.UtcNow);

        Assert.Throws<DomainRuleException>(() => sense.AddTranslation(
            Guid.NewGuid(),
            LanguageCode.From("en"),
            "train station",
            false,
            DateTime.UtcNow));
    }

    private static WordEntry CreateWordEntry()
    {
        return new WordEntry(
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
    }
}
