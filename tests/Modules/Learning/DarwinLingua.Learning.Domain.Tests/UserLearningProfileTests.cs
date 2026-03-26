using DarwinLingua.Learning.Domain.Entities;
using DarwinLingua.SharedKernel.Exceptions;
using DarwinLingua.SharedKernel.Globalization;

namespace DarwinLingua.Learning.Domain.Tests;

/// <summary>
/// Verifies the <see cref="UserLearningProfile"/> aggregate rules.
/// </summary>
public sealed class UserLearningProfileTests
{
    /// <summary>
    /// Verifies that the aggregate rejects duplicate primary and secondary meaning languages.
    /// </summary>
    [Fact]
    public void Constructor_ShouldRejectDuplicateMeaningLanguages()
    {
        Assert.Throws<DomainRuleException>(() => new UserLearningProfile(
            Guid.NewGuid(),
            "local-installation-user",
            LanguageCode.From("en"),
            LanguageCode.From("en"),
            LanguageCode.From("de"),
            DateTime.UtcNow));
    }

    /// <summary>
    /// Verifies that UI language updates refresh the last-updated timestamp.
    /// </summary>
    [Fact]
    public void UpdateUiLanguage_ShouldUpdateStoredLanguageAndTimestamp()
    {
        UserLearningProfile profile = new(
            Guid.NewGuid(),
            "local-installation-user",
            LanguageCode.From("en"),
            preferredMeaningLanguage2: null,
            LanguageCode.From("en"),
            DateTime.UtcNow.AddMinutes(-5));

        DateTime updatedAtUtc = DateTime.UtcNow;

        profile.UpdateUiLanguage(LanguageCode.From("de"), updatedAtUtc);

        Assert.Equal(LanguageCode.From("de"), profile.UiLanguageCode);
        Assert.Equal(updatedAtUtc, profile.UpdatedAtUtc);
    }
}
