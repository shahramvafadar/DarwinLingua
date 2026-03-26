using DarwinLingua.SharedKernel.Exceptions;
using DarwinLingua.SharedKernel.Globalization;

namespace DarwinLingua.Localization.Domain.Tests;

/// <summary>
/// Tests the <see cref="LanguageCode"/> value object behavior.
/// </summary>
public sealed class LanguageCodeTests
{
    /// <summary>
    /// Verifies that a language code is trimmed and normalized to lowercase.
    /// </summary>
    [Fact]
    public void From_ShouldNormalizeWhitespaceAndCasing()
    {
        LanguageCode code = LanguageCode.From(" EN ");

        Assert.Equal("en", code.Value);
    }

    /// <summary>
    /// Verifies that invalid language code values are rejected.
    /// </summary>
    [Fact]
    public void From_ShouldRejectInvalidValue()
    {
        Assert.Throws<DomainRuleException>(() => LanguageCode.From("english"));
    }
}
