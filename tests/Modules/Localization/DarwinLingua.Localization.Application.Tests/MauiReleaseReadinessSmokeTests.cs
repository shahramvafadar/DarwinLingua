namespace DarwinLingua.Localization.Application.Tests;

/// <summary>
/// Provides release-readiness smoke checks for MAUI localization wiring and offline-safe local flows.
/// </summary>
public sealed class MauiReleaseReadinessSmokeTests
{
    /// <summary>
    /// Verifies that the shell keeps localized tab titles and core navigation routes wired through AppStrings.
    /// </summary>
    [Fact]
    public void AppShell_ShouldKeepLocalizedTabsAndCoreRoutesConfigured()
    {
        string repositoryRoot = ResolveRepositoryRoot();
        string appShellPath = Path.Combine(repositoryRoot, "src/Apps/DarwinDeutsch.Maui/AppShell.xaml.cs");

        Assert.True(File.Exists(appShellPath), $"AppShell source file was not found: {appShellPath}");

        string sourceCode = File.ReadAllText(appShellPath);

        Assert.Contains("AppStrings.AppTitle", sourceCode, StringComparison.Ordinal);
        Assert.Contains("AppStrings.HomeTabTitle", sourceCode, StringComparison.Ordinal);
        Assert.Contains("AppStrings.BrowseTabTitle", sourceCode, StringComparison.Ordinal);
        Assert.Contains("AppStrings.FavoritesTabTitle", sourceCode, StringComparison.Ordinal);
        Assert.Contains("AppStrings.SettingsTabTitle", sourceCode, StringComparison.Ordinal);
        Assert.Contains("Routing.RegisterRoute(nameof(TopicWordsPage)", sourceCode, StringComparison.Ordinal);
        Assert.Contains("Routing.RegisterRoute(nameof(CefrWordsPage)", sourceCode, StringComparison.Ordinal);
        Assert.Contains("Routing.RegisterRoute(nameof(SearchWordsPage)", sourceCode, StringComparison.Ordinal);
        Assert.Contains("Routing.RegisterRoute(nameof(WordDetailPage)", sourceCode, StringComparison.Ordinal);
    }

    /// <summary>
    /// Verifies that core learner-facing pages remain localized and do not introduce direct network dependencies.
    /// </summary>
    [Fact]
    public void CorePages_ShouldStayLocalizedAndOfflineSafe()
    {
        string repositoryRoot = ResolveRepositoryRoot();

        string[] relativePaths =
        [
            "src/Apps/DarwinDeutsch.Maui/Pages/HomePage.xaml.cs",
            "src/Apps/DarwinDeutsch.Maui/Pages/TopicsPage.xaml.cs",
            "src/Apps/DarwinDeutsch.Maui/Pages/SearchWordsPage.xaml.cs",
            "src/Apps/DarwinDeutsch.Maui/Pages/TopicWordsPage.xaml.cs",
            "src/Apps/DarwinDeutsch.Maui/Pages/CefrWordsPage.xaml.cs",
            "src/Apps/DarwinDeutsch.Maui/Pages/FavoritesPage.xaml.cs",
            "src/Apps/DarwinDeutsch.Maui/Pages/WordDetailPage.xaml.cs",
            "src/Apps/DarwinDeutsch.Maui/Pages/SettingsPage.xaml.cs",
        ];

        foreach (string relativePath in relativePaths)
        {
            string fullPath = Path.Combine(repositoryRoot, relativePath);
            Assert.True(File.Exists(fullPath), $"MAUI page source file was not found: {fullPath}");

            string sourceCode = File.ReadAllText(fullPath);

            Assert.Contains("AppStrings.", sourceCode, StringComparison.Ordinal);
            Assert.DoesNotContain("HttpClient", sourceCode, StringComparison.Ordinal);
            Assert.DoesNotContain("IConnectivity", sourceCode, StringComparison.Ordinal);
            Assert.DoesNotContain("Connectivity.", sourceCode, StringComparison.Ordinal);
            Assert.DoesNotContain("Microsoft.Maui.Networking", sourceCode, StringComparison.Ordinal);
        }
    }

    /// <summary>
    /// Resolves the repository root path by walking parent directories.
    /// </summary>
    private static string ResolveRepositoryRoot()
    {
        DirectoryInfo? currentDirectory = new(AppContext.BaseDirectory);

        while (currentDirectory is not null)
        {
            string candidateSolutionPath = Path.Combine(currentDirectory.FullName, "DarwinLingua.slnx");

            if (File.Exists(candidateSolutionPath))
            {
                return currentDirectory.FullName;
            }

            currentDirectory = currentDirectory.Parent;
        }

        throw new InvalidOperationException("Unable to resolve repository root from test execution directory.");
    }
}
