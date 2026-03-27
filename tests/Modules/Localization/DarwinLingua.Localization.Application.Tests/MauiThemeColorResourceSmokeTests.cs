using System.Xml.Linq;

namespace DarwinLingua.Localization.Application.Tests;

/// <summary>
/// Guards application-scope MAUI theme color availability for parse-time XAML lookups.
/// </summary>
public sealed class MauiThemeColorResourceSmokeTests
{
    /// <summary>
    /// Verifies that App.xaml declares the core theme color keys required by startup pages and controls.
    /// </summary>
    [Fact]
    public void AppXaml_ShouldDeclareCoreThemeColorsAtApplicationScope()
    {
        string repositoryRoot = ResolveRepositoryRoot();
        string appXamlPath = Path.Combine(repositoryRoot, "src/Apps/DarwinDeutsch.Maui/App.xaml");

        Assert.True(File.Exists(appXamlPath), $"MAUI App.xaml file not found: {appXamlPath}");

        XDocument document = XDocument.Load(appXamlPath);
        XNamespace presentation = "http://schemas.microsoft.com/dotnet/2021/maui";
        XNamespace x = "http://schemas.microsoft.com/winfx/2009/xaml";

        HashSet<string> colorKeys = document
            .Descendants(presentation + "Color")
            .Select(element => element.Attribute(x + "Key")?.Value)
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Cast<string>()
            .ToHashSet(StringComparer.Ordinal);

        string[] requiredKeys =
        [
            "Secondary",
            "Gray950",
            "White",
            "MidnightBlue",
            "Gray500",
            "ColorCardStrokeLight",
            "ColorCardStrokeDark",
        ];

        foreach (string requiredKey in requiredKeys)
        {
            Assert.Contains(requiredKey, colorKeys);
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
