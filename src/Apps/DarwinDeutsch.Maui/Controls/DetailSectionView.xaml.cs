namespace DarwinDeutsch.Maui.Controls;

/// <summary>
/// Reusable detail-section view with a heading and body text.
/// </summary>
public partial class DetailSectionView : ContentView
{
    /// <summary>
    /// Backing bindable property for <see cref="SectionTitle"/>.
    /// </summary>
    public static readonly BindableProperty SectionTitleProperty = BindableProperty.Create(
        nameof(SectionTitle),
        typeof(string),
        typeof(DetailSectionView),
        string.Empty,
        propertyChanged: static (bindable, _, newValue) =>
        {
            ((DetailSectionView)bindable).SectionTitleLabel.Text = (string?)newValue ?? string.Empty;
        });

    /// <summary>
    /// Backing bindable property for <see cref="SectionValue"/>.
    /// </summary>
    public static readonly BindableProperty SectionValueProperty = BindableProperty.Create(
        nameof(SectionValue),
        typeof(string),
        typeof(DetailSectionView),
        string.Empty,
        propertyChanged: static (bindable, _, newValue) =>
        {
            ((DetailSectionView)bindable).SectionValueLabel.Text = (string?)newValue ?? string.Empty;
        });

    /// <summary>
    /// Initializes a new instance of the <see cref="DetailSectionView"/> class.
    /// </summary>
    public DetailSectionView()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the section heading.
    /// </summary>
    public string SectionTitle
    {
        get => (string)GetValue(SectionTitleProperty);
        set => SetValue(SectionTitleProperty, value);
    }

    /// <summary>
    /// Gets or sets the section body text.
    /// </summary>
    public string SectionValue
    {
        get => (string)GetValue(SectionValueProperty);
        set => SetValue(SectionValueProperty, value);
    }
}
