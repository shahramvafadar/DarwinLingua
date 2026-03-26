using System.Text.RegularExpressions;
using DarwinLingua.SharedKernel.Exceptions;

namespace DarwinLingua.Localization.Domain.ValueObjects;

/// <summary>
/// Represents a normalized language code used across the platform.
/// </summary>
public readonly partial record struct LanguageCode
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LanguageCode"/> struct.
    /// </summary>
    /// <param name="value">The normalized language code value.</param>
    private LanguageCode(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Gets the normalized language code value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Creates a normalized <see cref="LanguageCode"/> from the provided input.
    /// </summary>
    /// <param name="value">The raw language code value.</param>
    /// <returns>A normalized language code.</returns>
    public static LanguageCode From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainRuleException("Language code cannot be empty.");
        }

        string normalized = value.Trim().ToLowerInvariant();

        if (!LanguageCodePattern().IsMatch(normalized))
        {
            throw new DomainRuleException($"Language code '{value}' is invalid.");
        }

        return new LanguageCode(normalized);
    }

    /// <summary>
    /// Returns the normalized language code string.
    /// </summary>
    /// <returns>The normalized language code.</returns>
    public override string ToString()
    {
        return Value;
    }

    [GeneratedRegex("^[a-z]{2,3}(-[a-z0-9]{2,8})*$", RegexOptions.Compiled)]
    private static partial Regex LanguageCodePattern();
}
