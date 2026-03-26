using DarwinLingua.Localization.Domain.ValueObjects;
using DarwinLingua.SharedKernel.Exceptions;

namespace DarwinLingua.Localization.Domain.Entities;

/// <summary>
/// Represents a supported language record used by the platform.
/// </summary>
public sealed class Language
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Language"/> class for EF Core materialization.
    /// </summary>
    private Language()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Language"/> class.
    /// </summary>
    /// <param name="id">The stable internal identifier of the language.</param>
    /// <param name="code">The normalized language code.</param>
    /// <param name="englishName">The English display name of the language.</param>
    /// <param name="nativeName">The native display name of the language.</param>
    /// <param name="isActive">A value indicating whether the language is active.</param>
    /// <param name="supportsUserInterface">A value indicating whether the language supports UI localization.</param>
    /// <param name="supportsMeanings">A value indicating whether the language can be used for meaning translations.</param>
    public Language(
        Guid id,
        LanguageCode code,
        string englishName,
        string nativeName,
        bool isActive,
        bool supportsUserInterface,
        bool supportsMeanings)
    {
        if (id == Guid.Empty)
        {
            throw new DomainRuleException("Language identifier cannot be empty.");
        }

        string normalizedEnglishName = NormalizeRequiredText(englishName, nameof(englishName));
        string normalizedNativeName = NormalizeRequiredText(nativeName, nameof(nativeName));

        EnsureAtLeastOneCapability(supportsUserInterface, supportsMeanings);

        Id = id;
        Code = code;
        EnglishName = normalizedEnglishName;
        NativeName = normalizedNativeName;
        IsActive = isActive;
        SupportsUserInterface = supportsUserInterface;
        SupportsMeanings = supportsMeanings;
    }

    /// <summary>
    /// Gets the stable internal identifier of the language.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the normalized language code.
    /// </summary>
    public LanguageCode Code { get; private set; }

    /// <summary>
    /// Gets the English display name.
    /// </summary>
    public string EnglishName { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the native display name.
    /// </summary>
    public string NativeName { get; private set; } = string.Empty;

    /// <summary>
    /// Gets a value indicating whether the language is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the language supports UI localization.
    /// </summary>
    public bool SupportsUserInterface { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the language supports meaning translations.
    /// </summary>
    public bool SupportsMeanings { get; private set; }

    /// <summary>
    /// Activates the language.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
    }

    /// <summary>
    /// Deactivates the language.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
    }

    /// <summary>
    /// Updates the localized capabilities supported by this language.
    /// </summary>
    /// <param name="supportsUserInterface">A value indicating whether the language supports UI localization.</param>
    /// <param name="supportsMeanings">A value indicating whether the language can be used for meaning translations.</param>
    public void UpdateCapabilities(bool supportsUserInterface, bool supportsMeanings)
    {
        EnsureAtLeastOneCapability(supportsUserInterface, supportsMeanings);

        SupportsUserInterface = supportsUserInterface;
        SupportsMeanings = supportsMeanings;
    }

    /// <summary>
    /// Updates the display names of the language.
    /// </summary>
    /// <param name="englishName">The English display name.</param>
    /// <param name="nativeName">The native display name.</param>
    public void UpdateNames(string englishName, string nativeName)
    {
        EnglishName = NormalizeRequiredText(englishName, nameof(englishName));
        NativeName = NormalizeRequiredText(nativeName, nameof(nativeName));
    }

    /// <summary>
    /// Normalizes a required display text value.
    /// </summary>
    /// <param name="value">The incoming text value.</param>
    /// <param name="paramName">The originating parameter name.</param>
    /// <returns>The normalized non-empty text.</returns>
    private static string NormalizeRequiredText(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainRuleException($"{paramName} cannot be empty.");
        }

        return value.Trim();
    }

    /// <summary>
    /// Ensures that the language remains useful for at least one supported capability.
    /// </summary>
    /// <param name="supportsUserInterface">A value indicating whether the language supports UI localization.</param>
    /// <param name="supportsMeanings">A value indicating whether the language supports meaning translations.</param>
    private static void EnsureAtLeastOneCapability(bool supportsUserInterface, bool supportsMeanings)
    {
        if (!supportsUserInterface && !supportsMeanings)
        {
            throw new DomainRuleException("A language must support at least one platform capability.");
        }
    }
}
