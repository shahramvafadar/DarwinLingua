using DarwinLingua.Localization.Domain.Entities;
using DarwinLingua.SharedKernel.Exceptions;
using DarwinLingua.SharedKernel.Globalization;

namespace DarwinLingua.Localization.Domain.Tests;

/// <summary>
/// Tests the <see cref="Language"/> entity behavior.
/// </summary>
public sealed class LanguageTests
{
    /// <summary>
    /// Verifies that a language requires at least one active capability.
    /// </summary>
    [Fact]
    public void Constructor_ShouldRejectLanguageWithoutCapabilities()
    {
        Assert.Throws<DomainRuleException>(() =>
            new Language(
                Guid.NewGuid(),
                LanguageCode.From("en"),
                "English",
                "English",
                isActive: true,
                supportsUserInterface: false,
                supportsMeanings: false));
    }

    /// <summary>
    /// Verifies that capability updates still require at least one active capability.
    /// </summary>
    [Fact]
    public void UpdateCapabilities_ShouldRejectWhenAllCapabilitiesAreDisabled()
    {
        Language language = new(
            Guid.NewGuid(),
            LanguageCode.From("de"),
            "German",
            "Deutsch",
            isActive: true,
            supportsUserInterface: true,
            supportsMeanings: true);

        Assert.Throws<DomainRuleException>(() => language.UpdateCapabilities(false, false));
    }

    /// <summary>
    /// Verifies that display names are normalized when updated.
    /// </summary>
    [Fact]
    public void UpdateNames_ShouldTrimIncomingValues()
    {
        Language language = new(
            Guid.NewGuid(),
            LanguageCode.From("de"),
            "German",
            "Deutsch",
            isActive: true,
            supportsUserInterface: true,
            supportsMeanings: false);

        language.UpdateNames(" German ", " Deutsch ");

        Assert.Equal("German", language.EnglishName);
        Assert.Equal("Deutsch", language.NativeName);
    }
}
