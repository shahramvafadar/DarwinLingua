using DarwinDeutsch.Maui.Resources.Strings;
using DarwinDeutsch.Maui.Services.Localization;
using DarwinLingua.Localization.Application.Abstractions;
using DarwinLingua.Localization.Application.Models;

namespace DarwinDeutsch.Maui.Pages;

/// <summary>
/// Displays the first real application screen backed by localized strings and seeded reference data.
/// </summary>
public partial class HomePage : ContentPage
{
    private readonly IAppLocalizationService _appLocalizationService;
    private readonly ILanguageQueryService _languageQueryService;

    /// <summary>
    /// Initializes a new instance of the <see cref="HomePage"/> class.
    /// </summary>
    /// <param name="appLocalizationService">The service that manages UI localization.</param>
    /// <param name="languageQueryService">The service that reads seeded language reference data.</param>
    public HomePage(
        IAppLocalizationService appLocalizationService,
        ILanguageQueryService languageQueryService)
    {
        ArgumentNullException.ThrowIfNull(appLocalizationService);
        ArgumentNullException.ThrowIfNull(languageQueryService);

        InitializeComponent();

        _appLocalizationService = appLocalizationService;
        _languageQueryService = languageQueryService;

        _appLocalizationService.CultureChanged += OnCultureChanged;

        ApplyLocalizedText();
    }

    /// <summary>
    /// Refreshes localized copy and seeded data whenever the page becomes visible.
    /// </summary>
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        ApplyLocalizedText();
        await RefreshLanguageListAsync().ConfigureAwait(true);
    }

    /// <summary>
    /// Handles culture changes raised by the localization service.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void OnCultureChanged(object? sender, EventArgs e)
    {
        ApplyLocalizedText();

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await RefreshLanguageListAsync().ConfigureAwait(true);
        });
    }

    /// <summary>
    /// Applies localized text to the current visual elements.
    /// </summary>
    private void ApplyLocalizedText()
    {
        Title = AppStrings.HomeTabTitle;
        HeadlineLabel.Text = AppStrings.HomeHeadline;
        IntroLabel.Text = AppStrings.HomeIntro;
        CurrentLanguageCaptionLabel.Text = AppStrings.HomeCurrentUiLanguageLabel;
        CurrentLanguageValueLabel.Text = _appLocalizationService.CurrentCulture.NativeName;
        SupportedLanguagesCaptionLabel.Text = AppStrings.HomeSupportedLanguagesLabel;
    }

    /// <summary>
    /// Loads the active language rows that were seeded into the local database.
    /// </summary>
    /// <returns>A task that completes when the page values are refreshed.</returns>
    private async Task RefreshLanguageListAsync()
    {
        IReadOnlyList<SupportedLanguageModel> supportedLanguages = await _languageQueryService
            .GetActiveLanguagesAsync(CancellationToken.None)
            .ConfigureAwait(true);

        SupportedLanguagesValueLabel.Text = supportedLanguages.Count == 0
            ? AppStrings.HomeNoLanguages
            : string.Join(Environment.NewLine, supportedLanguages.Select(language =>
                $"{language.NativeName} ({language.Code})"));
    }
}
