using DarwinDeutsch.Maui.Resources.Strings;
using DarwinDeutsch.Maui.Services.Localization;

namespace DarwinDeutsch.Maui.Pages;

/// <summary>
/// Allows the user to override the UI language while preserving device-based defaults.
/// </summary>
public partial class SettingsPage : ContentPage
{
    private readonly IAppLocalizationService _appLocalizationService;
    private bool _isUpdatingSelection;

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsPage"/> class.
    /// </summary>
    /// <param name="appLocalizationService">The service that manages UI localization.</param>
    public SettingsPage(IAppLocalizationService appLocalizationService)
    {
        ArgumentNullException.ThrowIfNull(appLocalizationService);

        InitializeComponent();

        _appLocalizationService = appLocalizationService;
        _appLocalizationService.CultureChanged += OnCultureChanged;

        RebuildLocalizedState();
    }

    /// <summary>
    /// Rebuilds the localized page text when the page becomes visible.
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();

        RebuildLocalizedState();
    }

    /// <summary>
    /// Handles UI culture changes raised by the localization service.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void OnCultureChanged(object? sender, EventArgs e)
    {
        RebuildLocalizedState();
    }

    /// <summary>
    /// Applies the current localized copy and supported language options.
    /// </summary>
    private void RebuildLocalizedState()
    {
        IReadOnlyList<UiLanguageOption> supportedLanguages = _appLocalizationService.GetSupportedLanguages();

        Title = AppStrings.SettingsTabTitle;
        HeadlineLabel.Text = AppStrings.SettingsHeadline;
        DescriptionLabel.Text = AppStrings.SettingsDescription;
        LanguagePickerCaptionLabel.Text = AppStrings.SettingsUiLanguageLabel;

        _isUpdatingSelection = true;
        LanguagePicker.ItemsSource = supportedLanguages.ToList();
        LanguagePicker.SelectedItem = supportedLanguages.Single(option => string.Equals(
            option.CultureName,
            _appLocalizationService.CurrentCulture.TwoLetterISOLanguageName,
            StringComparison.OrdinalIgnoreCase));
        _isUpdatingSelection = false;
    }

    /// <summary>
    /// Persists the selected UI language when the picker value changes.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void OnLanguagePickerSelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_isUpdatingSelection || LanguagePicker.SelectedItem is not UiLanguageOption selectedLanguage)
        {
            return;
        }

        _appLocalizationService.SetCulture(selectedLanguage.CultureName);
    }
}
