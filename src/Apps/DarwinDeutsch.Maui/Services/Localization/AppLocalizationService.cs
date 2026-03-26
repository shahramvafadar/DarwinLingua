using System.Globalization;
using DarwinDeutsch.Maui.Resources.Strings;

namespace DarwinDeutsch.Maui.Services.Localization;

/// <summary>
/// Implements device-aware UI culture selection for the MAUI application.
/// </summary>
internal sealed class AppLocalizationService : IAppLocalizationService
{
    private const string UiLanguagePreferenceKey = "preferences.ui-language";
    private static readonly string[] SupportedCultureNames = ["en", "de"];

    /// <inheritdoc />
    public event EventHandler? CultureChanged;

    /// <inheritdoc />
    public CultureInfo CurrentCulture { get; private set; } = CultureInfo.InvariantCulture;

    /// <inheritdoc />
    public IReadOnlyList<UiLanguageOption> GetSupportedLanguages()
    {
        return
        [
            new UiLanguageOption("en", AppStrings.LanguageOptionEnglish),
            new UiLanguageOption("de", AppStrings.LanguageOptionGerman),
        ];
    }

    /// <inheritdoc />
    public void Initialize()
    {
        string? persistedCultureName = Preferences.Default.Get<string?>(UiLanguagePreferenceKey, null);
        CultureInfo selectedCulture = ResolveSupportedCulture(
            persistedCultureName is null ? CultureInfo.CurrentUICulture : new CultureInfo(persistedCultureName));

        ApplyCulture(selectedCulture, raiseEvent: false);
    }

    /// <inheritdoc />
    public void SetCulture(string cultureName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cultureName);

        CultureInfo selectedCulture = ResolveSupportedCulture(new CultureInfo(cultureName));

        Preferences.Default.Set(UiLanguagePreferenceKey, selectedCulture.Name);

        ApplyCulture(selectedCulture, raiseEvent: true);
    }

    /// <summary>
    /// Resolves the best supported culture from the incoming culture request.
    /// </summary>
    /// <param name="requestedCulture">The incoming requested culture.</param>
    /// <returns>The matched supported culture.</returns>
    private static CultureInfo ResolveSupportedCulture(CultureInfo requestedCulture)
    {
        ArgumentNullException.ThrowIfNull(requestedCulture);

        string? matchedCultureName = SupportedCultureNames
            .FirstOrDefault(cultureName => string.Equals(
                cultureName,
                requestedCulture.TwoLetterISOLanguageName,
                StringComparison.OrdinalIgnoreCase));

        return new CultureInfo(matchedCultureName ?? "en");
    }

    /// <summary>
    /// Applies the resolved culture to the current process and resource manager.
    /// </summary>
    /// <param name="culture">The resolved supported culture.</param>
    /// <param name="raiseEvent">A value indicating whether listeners should be notified.</param>
    private void ApplyCulture(CultureInfo culture, bool raiseEvent)
    {
        ArgumentNullException.ThrowIfNull(culture);

        CurrentCulture = culture;
        AppStrings.Culture = culture;
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;

        if (raiseEvent)
        {
            CultureChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
