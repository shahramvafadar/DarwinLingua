namespace DarwinDeutsch.Maui.Controls;

/// <summary>
/// Reusable list item view for lexical word summaries.
/// </summary>
public partial class WordListItemView : ContentView
{
    /// <summary>
    /// Backing bindable property for <see cref="Lemma"/>.
    /// </summary>
    public static readonly BindableProperty LemmaProperty = BindableProperty.Create(
        nameof(Lemma),
        typeof(string),
        typeof(WordListItemView),
        string.Empty,
        propertyChanged: static (bindable, _, newValue) =>
        {
            ((WordListItemView)bindable).LemmaLabel.Text = (string?)newValue ?? string.Empty;
        });

    /// <summary>
    /// Backing bindable property for <see cref="PrimaryMeaning"/>.
    /// </summary>
    public static readonly BindableProperty PrimaryMeaningProperty = BindableProperty.Create(
        nameof(PrimaryMeaning),
        typeof(string),
        typeof(WordListItemView),
        string.Empty,
        propertyChanged: static (bindable, _, newValue) =>
        {
            ((WordListItemView)bindable).PrimaryMeaningLabel.Text = (string?)newValue ?? string.Empty;
        });

    /// <summary>
    /// Backing bindable property for <see cref="MetadataLine"/>.
    /// </summary>
    public static readonly BindableProperty MetadataLineProperty = BindableProperty.Create(
        nameof(MetadataLine),
        typeof(string),
        typeof(WordListItemView),
        string.Empty,
        propertyChanged: static (bindable, _, newValue) =>
        {
            ((WordListItemView)bindable).MetadataLineLabel.Text = (string?)newValue ?? string.Empty;
        });

    /// <summary>
    /// Initializes a new instance of the <see cref="WordListItemView"/> class.
    /// </summary>
    public WordListItemView()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the lemma line text.
    /// </summary>
    public string Lemma
    {
        get => (string)GetValue(LemmaProperty);
        set => SetValue(LemmaProperty, value);
    }

    /// <summary>
    /// Gets or sets the primary meaning line text.
    /// </summary>
    public string PrimaryMeaning
    {
        get => (string)GetValue(PrimaryMeaningProperty);
        set => SetValue(PrimaryMeaningProperty, value);
    }

    /// <summary>
    /// Gets or sets the metadata line text.
    /// </summary>
    public string MetadataLine
    {
        get => (string)GetValue(MetadataLineProperty);
        set => SetValue(MetadataLineProperty, value);
    }
}
