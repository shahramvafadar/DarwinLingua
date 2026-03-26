namespace DarwinDeutsch.Maui.Services.Localization;

/// <summary>
/// Represents one selectable UI language option inside the MAUI application.
/// </summary>
public sealed class UiLanguageOption
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UiLanguageOption"/> class.
    /// </summary>
    /// <param name="cultureName">The supported culture name.</param>
    /// <param name="displayName">The localized display name shown to the user.</param>
    public UiLanguageOption(string cultureName, string displayName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cultureName);
        ArgumentException.ThrowIfNullOrWhiteSpace(displayName);

        CultureName = cultureName;
        DisplayName = displayName;
    }

    /// <summary>
    /// Gets the supported culture name.
    /// </summary>
    public string CultureName { get; }

    /// <summary>
    /// Gets the localized display name shown to the user.
    /// </summary>
    public string DisplayName { get; }
}
