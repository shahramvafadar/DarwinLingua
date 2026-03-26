namespace DarwinDeutsch.Maui;

/// <summary>
/// Represents the MAUI application entry point.
/// </summary>
public partial class App : Application
{
    private readonly AppShell _appShell;

    /// <summary>
    /// Initializes a new instance of the <see cref="App"/> class.
    /// </summary>
    /// <param name="appShell">The application shell resolved from dependency injection.</param>
    public App(AppShell appShell)
    {
        ArgumentNullException.ThrowIfNull(appShell);

        InitializeComponent();

        _appShell = appShell;
    }

    /// <summary>
    /// Creates the main application window.
    /// </summary>
    /// <param name="activationState">The optional platform activation state.</param>
    /// <returns>The configured application window.</returns>
    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(_appShell);
    }
}
