using System.Globalization;

namespace DarwinDeutsch.Maui.Services.Localization;

/// <summary>
/// Coordinates UI culture selection and localized string access inside the MAUI host.
/// </summary>
public interface IAppLocalizationService
{
    /// <summary>
    /// Occurs when the active UI culture changes.
    /// </summary>
    event EventHandler? CultureChanged;

    /// <summary>
    /// Gets the currently active UI culture.
    /// </summary>
    CultureInfo CurrentCulture { get; }

    /// <summary>
    /// Returns the supported UI language options for the settings screen.
    /// </summary>
    /// <returns>The supported UI language options.</returns>
    IReadOnlyList<UiLanguageOption> GetSupportedLanguages();

    /// <summary>
    /// Initializes the UI culture from persisted settings or the current device culture.
    /// </summary>
    void Initialize();

    /// <summary>
    /// Changes the active UI culture and persists the selection.
    /// </summary>
    /// <param name="cultureName">The selected supported culture name.</param>
    void SetCulture(string cultureName);
}
