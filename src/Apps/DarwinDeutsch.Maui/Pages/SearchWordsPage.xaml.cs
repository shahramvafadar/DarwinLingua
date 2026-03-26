using DarwinDeutsch.Maui.Resources.Strings;
using DarwinLingua.Catalog.Application.Abstractions;
using DarwinLingua.Catalog.Application.Models;
using DarwinLingua.Learning.Application.Abstractions;
using DarwinLingua.Learning.Application.Models;

namespace DarwinDeutsch.Maui.Pages;

/// <summary>
/// Displays local lexical search results for German lemma queries.
/// </summary>
public partial class SearchWordsPage : ContentPage
{
    private readonly IWordQueryService _wordQueryService;
    private readonly IUserLearningProfileService _userLearningProfileService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchWordsPage"/> class.
    /// </summary>
    public SearchWordsPage(IWordQueryService wordQueryService, IUserLearningProfileService userLearningProfileService)
    {
        ArgumentNullException.ThrowIfNull(wordQueryService);
        ArgumentNullException.ThrowIfNull(userLearningProfileService);

        InitializeComponent();

        _wordQueryService = wordQueryService;
        _userLearningProfileService = userLearningProfileService;

        ApplyLocalizedText();
    }

    /// <summary>
    /// Re-applies localized text whenever the page becomes visible.
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();

        ApplyLocalizedText();
    }

    /// <summary>
    /// Executes the search when the user submits the search bar.
    /// </summary>
    private async void OnSearchButtonPressed(object? sender, EventArgs e)
    {
        await SearchAsync().ConfigureAwait(true);
    }

    /// <summary>
    /// Navigates to the selected lexical-entry detail page.
    /// </summary>
    private async void OnWordsSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not SearchWordItemViewModel selectedWord)
        {
            return;
        }

        WordsCollectionView.SelectedItem = null;

        string wordPublicId = Uri.EscapeDataString(selectedWord.PublicId.ToString());
        await Shell.Current.GoToAsync($"{nameof(WordDetailPage)}?wordPublicId={wordPublicId}")
            .ConfigureAwait(true);
    }

    /// <summary>
    /// Applies the localized static text values to the page.
    /// </summary>
    private void ApplyLocalizedText()
    {
        Title = AppStrings.SearchWordsPageTitle;
        HeadlineLabel.Text = AppStrings.SearchWordsPageHeadline;
        DescriptionLabel.Text = AppStrings.SearchWordsPageDescription;
        SearchBarControl.Placeholder = AppStrings.SearchWordsPagePlaceholder;
        EmptyStateLabel.Text = AppStrings.SearchWordsPageEmpty;
    }

    /// <summary>
    /// Executes the current search query against the local lexical store.
    /// </summary>
    private async Task SearchAsync()
    {
        UserLearningProfileModel profile = await _userLearningProfileService
            .GetCurrentProfileAsync(CancellationToken.None)
            .ConfigureAwait(true);

        string query = SearchBarControl.Text ?? string.Empty;

        if (string.IsNullOrWhiteSpace(query))
        {
            ShowResults(Array.Empty<SearchWordItemViewModel>());
            return;
        }

        IReadOnlyList<WordListItemModel> words = await _wordQueryService
            .SearchWordsAsync(query, profile.PreferredMeaningLanguage1, CancellationToken.None)
            .ConfigureAwait(true);

        ShowResults(words
            .Select(word => new SearchWordItemViewModel(
                word.PublicId,
                string.IsNullOrWhiteSpace(word.Article) ? word.Lemma : $"{word.Article} {word.Lemma}",
                word.PrimaryMeaning ?? AppStrings.TopicWordsPageMeaningUnavailable,
                $"{word.PartOfSpeech} · {word.CefrLevel}"))
            .ToArray());
    }

    /// <summary>
    /// Applies the current search result set to the page.
    /// </summary>
    private void ShowResults(IReadOnlyList<SearchWordItemViewModel> words)
    {
        WordsCollectionView.ItemsSource = words;
        EmptyStateLabel.IsVisible = words.Count == 0;
        WordsCollectionView.IsVisible = words.Count > 0;
    }

    /// <summary>
    /// Represents the UI model used by the search results collection view.
    /// </summary>
    private sealed record SearchWordItemViewModel(Guid PublicId, string Lemma, string PrimaryMeaning, string MetadataLine);
}
