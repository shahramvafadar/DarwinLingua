using DarwinDeutsch.Maui.Resources.Strings;
using DarwinLingua.Catalog.Application.Abstractions;
using DarwinLingua.Catalog.Application.Models;
using DarwinLingua.Learning.Application.Abstractions;
using DarwinLingua.Learning.Application.Models;

namespace DarwinDeutsch.Maui.Pages;

/// <summary>
/// Displays the detail view of a selected lexical entry.
/// </summary>
[QueryProperty(nameof(WordPublicId), "wordPublicId")]
public partial class WordDetailPage : ContentPage
{
    private readonly IWordDetailQueryService _wordDetailQueryService;
    private readonly IUserLearningProfileService _userLearningProfileService;
    private readonly IUserFavoriteWordService _userFavoriteWordService;
    private string _wordPublicId = string.Empty;
    private bool _isFavorite;

    /// <summary>
    /// Initializes a new instance of the <see cref="WordDetailPage"/> class.
    /// </summary>
    public WordDetailPage(
        IWordDetailQueryService wordDetailQueryService,
        IUserLearningProfileService userLearningProfileService,
        IUserFavoriteWordService userFavoriteWordService)
    {
        ArgumentNullException.ThrowIfNull(wordDetailQueryService);
        ArgumentNullException.ThrowIfNull(userLearningProfileService);
        ArgumentNullException.ThrowIfNull(userFavoriteWordService);

        InitializeComponent();

        _wordDetailQueryService = wordDetailQueryService;
        _userLearningProfileService = userLearningProfileService;
        _userFavoriteWordService = userFavoriteWordService;
    }

    /// <summary>
    /// Gets or sets the public identifier of the selected lexical entry.
    /// </summary>
    public string WordPublicId
    {
        get => _wordPublicId;
        set => _wordPublicId = Uri.UnescapeDataString(value ?? string.Empty);
    }

    /// <summary>
    /// Refreshes the detail page whenever it becomes visible.
    /// </summary>
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await RefreshAsync().ConfigureAwait(true);
    }

    /// <summary>
    /// Loads the selected lexical entry using the current profile language preferences.
    /// </summary>
    private async Task RefreshAsync()
    {
        TopicsCaptionLabel.Text = AppStrings.WordDetailTopicsLabel;
        EmptyStateLabel.Text = AppStrings.WordDetailNotFound;
        SensesContainer.Children.Clear();
        FavoriteButton.IsVisible = false;

        if (!Guid.TryParse(WordPublicId, out Guid publicId))
        {
            ShowNotFoundState();
            return;
        }

        UserLearningProfileModel profile = await _userLearningProfileService
            .GetCurrentProfileAsync(CancellationToken.None)
            .ConfigureAwait(true);

        WordDetailModel? word = await _wordDetailQueryService
            .GetWordDetailsAsync(
                publicId,
                profile.PreferredMeaningLanguage1,
                profile.PreferredMeaningLanguage2,
                profile.UiLanguageCode,
                CancellationToken.None)
            .ConfigureAwait(true);

        if (word is null)
        {
            ShowNotFoundState();
            return;
        }

        _isFavorite = await _userFavoriteWordService
            .IsFavoriteAsync(publicId, CancellationToken.None)
            .ConfigureAwait(true);

        Title = BuildHeadline(word);
        HeadlineLabel.Text = BuildHeadline(word);
        MetadataLabel.Text = $"{word.PartOfSpeech} · {word.CefrLevel}";
        FavoriteButton.Text = _isFavorite
            ? AppStrings.WordDetailRemoveFavoriteButton
            : AppStrings.WordDetailAddFavoriteButton;
        FavoriteButton.IsVisible = true;
        TopicsValueLabel.Text = word.Topics.Count == 0
            ? AppStrings.WordDetailNoTopics
            : string.Join(", ", word.Topics);
        EmptyStateLabel.IsVisible = false;

        foreach (WordSenseDetailModel sense in word.Senses)
        {
            SensesContainer.Children.Add(BuildSenseView(sense));
        }
    }

    /// <summary>
    /// Shows the not-found state when the selected word is unavailable.
    /// </summary>
    private void ShowNotFoundState()
    {
        Title = AppStrings.WordDetailTitle;
        HeadlineLabel.Text = AppStrings.WordDetailTitle;
        MetadataLabel.Text = string.Empty;
        FavoriteButton.IsVisible = false;
        TopicsValueLabel.Text = string.Empty;
        EmptyStateLabel.IsVisible = true;
    }

    /// <summary>
    /// Toggles the favorite state of the current lexical entry.
    /// </summary>
    private async void OnFavoriteButtonClicked(object? sender, EventArgs e)
    {
        if (!Guid.TryParse(WordPublicId, out Guid publicId))
        {
            return;
        }

        _isFavorite = await _userFavoriteWordService
            .ToggleFavoriteAsync(publicId, CancellationToken.None)
            .ConfigureAwait(true);

        FavoriteButton.Text = _isFavorite
            ? AppStrings.WordDetailRemoveFavoriteButton
            : AppStrings.WordDetailAddFavoriteButton;
    }

    /// <summary>
    /// Builds the visual block for a single sense.
    /// </summary>
    private static View BuildSenseView(WordSenseDetailModel sense)
    {
        ArgumentNullException.ThrowIfNull(sense);

        VerticalStackLayout senseLayout = new()
        {
            Spacing = 8,
        };

        if (!string.IsNullOrWhiteSpace(sense.ShortDefinitionDe))
        {
            senseLayout.Children.Add(new Label
            {
                Text = sense.ShortDefinitionDe,
                Style = (Style)Application.Current!.Resources["Title2"],
            });
        }

        if (!string.IsNullOrWhiteSpace(sense.PrimaryMeaning))
        {
            senseLayout.Children.Add(new Label
            {
                Text = sense.PrimaryMeaning,
                Style = (Style)Application.Current!.Resources["Body"],
            });
        }

        if (!string.IsNullOrWhiteSpace(sense.SecondaryMeaning))
        {
            senseLayout.Children.Add(new Label
            {
                Text = sense.SecondaryMeaning,
                Style = (Style)Application.Current!.Resources["Body"],
            });
        }

        foreach (ExampleSentenceDetailModel example in sense.Examples)
        {
            VerticalStackLayout exampleLayout = new()
            {
                Spacing = 2,
                Margin = new Thickness(8, 0, 0, 0),
            };

            exampleLayout.Children.Add(new Label
            {
                Text = example.GermanText,
                Style = (Style)Application.Current!.Resources["Body"],
            });

            if (!string.IsNullOrWhiteSpace(example.PrimaryMeaning))
            {
                exampleLayout.Children.Add(new Label
                {
                    Text = example.PrimaryMeaning,
                    Style = (Style)Application.Current!.Resources["Body"],
                });
            }

            if (!string.IsNullOrWhiteSpace(example.SecondaryMeaning))
            {
                exampleLayout.Children.Add(new Label
                {
                    Text = example.SecondaryMeaning,
                    Style = (Style)Application.Current!.Resources["Body"],
                });
            }

            senseLayout.Children.Add(exampleLayout);
        }

        return new Border
        {
            Padding = 16,
            Content = senseLayout,
        };
    }

    /// <summary>
    /// Builds the page headline for the current lexical entry.
    /// </summary>
    private static string BuildHeadline(WordDetailModel word)
    {
        ArgumentNullException.ThrowIfNull(word);

        return string.IsNullOrWhiteSpace(word.Article)
            ? word.Lemma
            : $"{word.Article} {word.Lemma}";
    }
}
