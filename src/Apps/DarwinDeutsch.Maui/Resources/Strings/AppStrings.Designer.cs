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
    /// Gets the localized title for the favorites tab.
    /// </summary>
    public static string FavoritesTabTitle => GetRequiredString(nameof(FavoritesTabTitle));

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
    /// Gets the localized label for the meaning-language preferences section.
    /// </summary>
    public static string HomeMeaningLanguagesLabel => GetRequiredString(nameof(HomeMeaningLanguagesLabel));

    /// <summary>
    /// Gets the localized placeholder shown when no languages are available.
    /// </summary>
    public static string HomeNoLanguages => GetRequiredString(nameof(HomeNoLanguages));

    /// <summary>
    /// Gets the localized label for the seeded topics section.
    /// </summary>
    public static string HomeTopicsLabel => GetRequiredString(nameof(HomeTopicsLabel));

    /// <summary>
    /// Gets the localized label for the CEFR browse section.
    /// </summary>
    public static string HomeCefrBrowseLabel => GetRequiredString(nameof(HomeCefrBrowseLabel));

    /// <summary>
    /// Gets the localized label for the search section.
    /// </summary>
    public static string HomeSearchLabel => GetRequiredString(nameof(HomeSearchLabel));

    /// <summary>
    /// Gets the localized caption for the search navigation button.
    /// </summary>
    public static string HomeSearchButton => GetRequiredString(nameof(HomeSearchButton));

    /// <summary>
    /// Gets the localized placeholder shown when no topics are available.
    /// </summary>
    public static string HomeNoTopics => GetRequiredString(nameof(HomeNoTopics));

    /// <summary>
    /// Gets the localized title for the browse tab.
    /// </summary>
    public static string BrowseTabTitle => GetRequiredString(nameof(BrowseTabTitle));

    /// <summary>
    /// Gets the localized headline for the topics page.
    /// </summary>
    public static string TopicsPageHeadline => GetRequiredString(nameof(TopicsPageHeadline));

    /// <summary>
    /// Gets the localized description for the topics page.
    /// </summary>
    public static string TopicsPageDescription => GetRequiredString(nameof(TopicsPageDescription));

    /// <summary>
    /// Gets the localized empty state for the topics page.
    /// </summary>
    public static string TopicsPageEmpty => GetRequiredString(nameof(TopicsPageEmpty));

    /// <summary>
    /// Gets the localized title for the topic-words page.
    /// </summary>
    public static string TopicWordsPageTitle => GetRequiredString(nameof(TopicWordsPageTitle));

    /// <summary>
    /// Gets the localized headline format for the topic-words page.
    /// </summary>
    public static string TopicWordsPageHeadlineFormat => GetRequiredString(nameof(TopicWordsPageHeadlineFormat));

    /// <summary>
    /// Gets the localized description for the topic-words page.
    /// </summary>
    public static string TopicWordsPageDescription => GetRequiredString(nameof(TopicWordsPageDescription));

    /// <summary>
    /// Gets the localized empty state for the topic-words page.
    /// </summary>
    public static string TopicWordsPageEmpty => GetRequiredString(nameof(TopicWordsPageEmpty));

    /// <summary>
    /// Gets the localized placeholder shown when no preferred-language meaning is available.
    /// </summary>
    public static string TopicWordsPageMeaningUnavailable => GetRequiredString(nameof(TopicWordsPageMeaningUnavailable));

    /// <summary>
    /// Gets the localized default title for the word-detail page.
    /// </summary>
    public static string WordDetailTitle => GetRequiredString(nameof(WordDetailTitle));

    /// <summary>
    /// Gets the localized caption for adding the current word to favorites.
    /// </summary>
    public static string WordDetailAddFavoriteButton => GetRequiredString(nameof(WordDetailAddFavoriteButton));

    /// <summary>
    /// Gets the localized caption for removing the current word from favorites.
    /// </summary>
    public static string WordDetailRemoveFavoriteButton => GetRequiredString(nameof(WordDetailRemoveFavoriteButton));

    /// <summary>
    /// Gets the localized label for the topic section on the word-detail page.
    /// </summary>
    public static string WordDetailTopicsLabel => GetRequiredString(nameof(WordDetailTopicsLabel));

    /// <summary>
    /// Gets the localized empty-topic message for the word-detail page.
    /// </summary>
    public static string WordDetailNoTopics => GetRequiredString(nameof(WordDetailNoTopics));

    /// <summary>
    /// Gets the localized not-found state for the word-detail page.
    /// </summary>
    public static string WordDetailNotFound => GetRequiredString(nameof(WordDetailNotFound));

    /// <summary>
    /// Gets the localized title for the CEFR browse page.
    /// </summary>
    public static string CefrWordsPageTitle => GetRequiredString(nameof(CefrWordsPageTitle));

    /// <summary>
    /// Gets the localized headline format for the CEFR browse page.
    /// </summary>
    public static string CefrWordsPageHeadlineFormat => GetRequiredString(nameof(CefrWordsPageHeadlineFormat));

    /// <summary>
    /// Gets the localized description for the CEFR browse page.
    /// </summary>
    public static string CefrWordsPageDescription => GetRequiredString(nameof(CefrWordsPageDescription));

    /// <summary>
    /// Gets the localized empty state for the CEFR browse page.
    /// </summary>
    public static string CefrWordsPageEmpty => GetRequiredString(nameof(CefrWordsPageEmpty));

    /// <summary>
    /// Gets the localized title for the search page.
    /// </summary>
    public static string SearchWordsPageTitle => GetRequiredString(nameof(SearchWordsPageTitle));

    /// <summary>
    /// Gets the localized headline for the search page.
    /// </summary>
    public static string SearchWordsPageHeadline => GetRequiredString(nameof(SearchWordsPageHeadline));

    /// <summary>
    /// Gets the localized description for the search page.
    /// </summary>
    public static string SearchWordsPageDescription => GetRequiredString(nameof(SearchWordsPageDescription));

    /// <summary>
    /// Gets the localized placeholder for the search bar.
    /// </summary>
    public static string SearchWordsPagePlaceholder => GetRequiredString(nameof(SearchWordsPagePlaceholder));

    /// <summary>
    /// Gets the localized empty state for the search page.
    /// </summary>
    public static string SearchWordsPageEmpty => GetRequiredString(nameof(SearchWordsPageEmpty));

    /// <summary>
    /// Gets the localized title for the favorites page.
    /// </summary>
    public static string FavoritesPageTitle => GetRequiredString(nameof(FavoritesPageTitle));

    /// <summary>
    /// Gets the localized headline for the favorites page.
    /// </summary>
    public static string FavoritesPageHeadline => GetRequiredString(nameof(FavoritesPageHeadline));

    /// <summary>
    /// Gets the localized description for the favorites page.
    /// </summary>
    public static string FavoritesPageDescription => GetRequiredString(nameof(FavoritesPageDescription));

    /// <summary>
    /// Gets the localized empty state for the favorites page.
    /// </summary>
    public static string FavoritesPageEmpty => GetRequiredString(nameof(FavoritesPageEmpty));

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
    /// Gets the localized picker title for the primary meaning language selector.
    /// </summary>
    public static string SettingsPrimaryMeaningLanguageLabel => GetRequiredString(nameof(SettingsPrimaryMeaningLanguageLabel));

    /// <summary>
    /// Gets the localized picker title for the secondary meaning language selector.
    /// </summary>
    public static string SettingsSecondaryMeaningLanguageLabel => GetRequiredString(nameof(SettingsSecondaryMeaningLanguageLabel));

    /// <summary>
    /// Gets the localized display text for the empty secondary meaning-language option.
    /// </summary>
    public static string SettingsSecondaryMeaningLanguageNone => GetRequiredString(nameof(SettingsSecondaryMeaningLanguageNone));

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
