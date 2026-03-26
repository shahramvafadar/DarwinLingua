#nullable enable
using System.Globalization;
using System.Resources;

namespace DarwinDeutsch.Maui.Resources.Strings;

/// <summary>
/// Provides strongly typed access to localized UI resources.
/// </summary>
public static class AppStrings
{
    private static readonly ResourceManager ResourceManagerInstance =
        new("DarwinDeutsch.Maui.Resources.Strings.AppStrings", typeof(AppStrings).Assembly);

    /// <summary>
    /// Gets or sets the culture used by the resource manager.
    /// </summary>
    public static CultureInfo? Culture { get; set; }

    /// <summary>
    /// Gets the localized application title.
    /// </summary>
    public static string AppTitle => GetRequiredString(nameof(AppTitle));

    /// <summary>
    /// Gets the localized title for the home tab.
    /// </summary>
    public static string HomeTabTitle => GetRequiredString(nameof(HomeTabTitle));

    /// <summary>
    /// Gets the localized title for the settings tab.
    /// </summary>
    public static string SettingsTabTitle => GetRequiredString(nameof(SettingsTabTitle));

    /// <summary>
    /// Gets the localized headline for the home page.
    /// </summary>
    public static string HomeHeadline => GetRequiredString(nameof(HomeHeadline));

    /// <summary>
    /// Gets the localized introduction shown on the home page.
    /// </summary>
    public static string HomeIntro => GetRequiredString(nameof(HomeIntro));

    /// <summary>
    /// Gets the localized label for the current UI language section.
    /// </summary>
    public static string HomeCurrentUiLanguageLabel => GetRequiredString(nameof(HomeCurrentUiLanguageLabel));

    /// <summary>
    /// Gets the localized label for the supported languages section.
    /// </summary>
    public static string HomeSupportedLanguagesLabel => GetRequiredString(nameof(HomeSupportedLanguagesLabel));

    /// <summary>
    /// Gets the localized placeholder shown when no languages are available.
    /// </summary>
    public static string HomeNoLanguages => GetRequiredString(nameof(HomeNoLanguages));

    /// <summary>
    /// Gets the localized headline for the settings page.
    /// </summary>
    public static string SettingsHeadline => GetRequiredString(nameof(SettingsHeadline));

    /// <summary>
    /// Gets the localized description shown on the settings page.
    /// </summary>
    public static string SettingsDescription => GetRequiredString(nameof(SettingsDescription));

    /// <summary>
    /// Gets the localized picker title for the UI language selector.
    /// </summary>
    public static string SettingsUiLanguageLabel => GetRequiredString(nameof(SettingsUiLanguageLabel));

    /// <summary>
    /// Gets the localized display label for English.
    /// </summary>
    public static string LanguageOptionEnglish => GetRequiredString(nameof(LanguageOptionEnglish));

    /// <summary>
    /// Gets the localized display label for German.
    /// </summary>
    public static string LanguageOptionGerman => GetRequiredString(nameof(LanguageOptionGerman));

    /// <summary>
    /// Gets a required localized string by key.
    /// </summary>
    /// <param name="name">The resource key name.</param>
    /// <returns>The localized string value.</returns>
    private static string GetRequiredString(string name)
    {
        string? value = ResourceManagerInstance.GetString(name, Culture);

        return value ?? throw new MissingManifestResourceException($"Missing resource value for '{name}'.");
    }
}
#nullable restore
