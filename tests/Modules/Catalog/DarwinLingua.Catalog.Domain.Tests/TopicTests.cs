using DarwinLingua.Catalog.Domain.Entities;
using DarwinLingua.SharedKernel.Exceptions;
using DarwinLingua.SharedKernel.Globalization;

namespace DarwinLingua.Catalog.Domain.Tests;

/// <summary>
/// Tests the <see cref="Topic"/> aggregate behavior.
/// </summary>
public sealed class TopicTests
{
    /// <summary>
    /// Verifies that duplicate localizations for the same language are merged rather than duplicated.
    /// </summary>
    [Fact]
    public void AddOrUpdateLocalization_ShouldUpdateExistingLanguageRow()
    {
        Topic topic = new(Guid.NewGuid(), "shopping", 10, true, DateTime.UtcNow);

        topic.AddOrUpdateLocalization(Guid.NewGuid(), LanguageCode.From("en"), "Shopping", DateTime.UtcNow);
        topic.AddOrUpdateLocalization(Guid.Empty, LanguageCode.From("en"), "Everyday Shopping", DateTime.UtcNow);

        TopicLocalization localization = Assert.Single(topic.Localizations);
        Assert.Equal("Everyday Shopping", localization.DisplayName);
    }

    /// <summary>
    /// Verifies that invalid topic keys are rejected.
    /// </summary>
    [Fact]
    public void Constructor_ShouldRejectInvalidKey()
    {
        Assert.Throws<DomainRuleException>(() =>
            new Topic(Guid.NewGuid(), "Shopping List", 0, true, DateTime.UtcNow));
    }
}
