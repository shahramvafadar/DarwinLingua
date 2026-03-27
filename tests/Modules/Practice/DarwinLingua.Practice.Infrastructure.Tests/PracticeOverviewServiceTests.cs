using DarwinLingua.Catalog.Application.Abstractions;
using DarwinLingua.Catalog.Application.DependencyInjection;
using DarwinLingua.Catalog.Infrastructure.DependencyInjection;
using DarwinLingua.ContentOps.Application.Abstractions;
using DarwinLingua.ContentOps.Application.DependencyInjection;
using DarwinLingua.ContentOps.Application.Models;
using DarwinLingua.ContentOps.Infrastructure.DependencyInjection;
using DarwinLingua.Infrastructure.DependencyInjection;
using DarwinLingua.Infrastructure.Persistence.Abstractions;
using DarwinLingua.Learning.Application.Abstractions;
using DarwinLingua.Learning.Application.DependencyInjection;
using DarwinLingua.Learning.Infrastructure.DependencyInjection;
using DarwinLingua.Localization.Application.DependencyInjection;
using DarwinLingua.Localization.Infrastructure.DependencyInjection;
using DarwinLingua.Practice.Application.Abstractions;
using DarwinLingua.Practice.Application.DependencyInjection;
using DarwinLingua.Practice.Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace DarwinLingua.Practice.Infrastructure.Tests;

/// <summary>
/// Verifies the first Phase 2 practice-overview workflow against a temporary SQLite database.
/// </summary>
public sealed class PracticeOverviewServiceTests
{
    /// <summary>
    /// Verifies that imported content plus user-word-state interactions produce a meaningful practice overview.
    /// </summary>
    [Fact]
    public async Task GetOverviewAsync_ShouldSummarizeTrackedWordsAndReviewCandidates()
    {
        string databasePath = Path.Combine(Path.GetTempPath(), $"darwin-lingua-practice-{Guid.NewGuid():N}.db");
        string packagePath = Path.Combine(Path.GetTempPath(), $"darwin-lingua-practice-package-{Guid.NewGuid():N}.json");
        ServiceProvider? serviceProvider = null;

        try
        {
            File.Copy(GetSamplePackagePath(), packagePath, overwrite: true);

            serviceProvider = BuildServiceProvider(databasePath);

            IDatabaseInitializer databaseInitializer = serviceProvider.GetRequiredService<IDatabaseInitializer>();
            await databaseInitializer.InitializeAsync(CancellationToken.None);

            IContentImportService contentImportService = serviceProvider.GetRequiredService<IContentImportService>();
            ImportContentPackageResult importResult = await contentImportService
                .ImportAsync(new ImportContentPackageRequest(packagePath), CancellationToken.None);

            Assert.True(importResult.IsSuccess);

            IWordQueryService wordQueryService = serviceProvider.GetRequiredService<IWordQueryService>();
            IUserWordStateService userWordStateService = serviceProvider.GetRequiredService<IUserWordStateService>();
            IPracticeOverviewService practiceOverviewService = serviceProvider.GetRequiredService<IPracticeOverviewService>();

            DarwinLingua.Catalog.Application.Models.WordListItemModel breadWord = Assert.Single(await wordQueryService
                .SearchWordsAsync("Brot", "en", CancellationToken.None));
            DarwinLingua.Catalog.Application.Models.WordListItemModel applicationWord = Assert.Single(await wordQueryService
                .SearchWordsAsync("Bewerbung", "en", CancellationToken.None));
            DarwinLingua.Catalog.Application.Models.WordListItemModel indispensabilityWord = Assert.Single(await wordQueryService
                .SearchWordsAsync("Unabdingbarkeit", "en", CancellationToken.None));

            await userWordStateService.TrackWordViewedAsync(breadWord.PublicId, CancellationToken.None);
            await userWordStateService.MarkWordKnownAsync(breadWord.PublicId, CancellationToken.None);

            await userWordStateService.TrackWordViewedAsync(applicationWord.PublicId, CancellationToken.None);
            await userWordStateService.MarkWordDifficultAsync(applicationWord.PublicId, CancellationToken.None);

            await userWordStateService.TrackWordViewedAsync(indispensabilityWord.PublicId, CancellationToken.None);

            DarwinLingua.Practice.Application.Models.PracticeOverviewModel overview = await practiceOverviewService
                .GetOverviewAsync("en", CancellationToken.None);

            Assert.Equal(3, overview.TotalTrackedWords);
            Assert.Equal(2, overview.ReviewCandidateCount);
            Assert.Equal(1, overview.DifficultWordCount);
            Assert.Equal(1, overview.KnownWordCount);
            Assert.Equal(3, overview.RecentlyViewedCount);
            Assert.NotNull(overview.LastActivityAtUtc);
            Assert.Equal(2, overview.ReviewPreview.Count);
            Assert.Equal("Bewerbung", overview.ReviewPreview[0].Lemma);
            Assert.Equal("job application", overview.ReviewPreview[0].PrimaryMeaning);
            Assert.True(overview.ReviewPreview[0].IsDifficult);
            Assert.Contains(
                overview.ReviewPreview,
                item => item.Lemma == "Unabdingbarkeit" &&
                    item.PrimaryMeaning == "indispensability" &&
                    !item.IsKnown);
        }
        finally
        {
            if (serviceProvider is not null)
            {
                await serviceProvider.DisposeAsync();
            }

            if (File.Exists(packagePath))
            {
                File.Delete(packagePath);
            }
        }
    }

    private static ServiceProvider BuildServiceProvider(string databasePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(databasePath);

        ServiceCollection services = new();
        services
            .AddDarwinLinguaInfrastructure(options => options.DatabasePath = databasePath)
            .AddCatalogApplication()
            .AddCatalogInfrastructure()
            .AddContentOpsApplication()
            .AddContentOpsInfrastructure()
            .AddLearningApplication()
            .AddLearningInfrastructure()
            .AddLocalizationApplication()
            .AddLocalizationInfrastructure()
            .AddPracticeApplication()
            .AddPracticeInfrastructure();

        return services.BuildServiceProvider();
    }

    private static string GetSamplePackagePath()
    {
        string repositoryRoot = ResolveRepositoryRoot();
        string samplePackagePath = Path.Combine(
            repositoryRoot,
            "tests/Modules/ContentOps/DarwinLingua.ContentOps.Infrastructure.Tests/Fixtures/phase1-sample-content-package.json");

        Assert.True(File.Exists(samplePackagePath), $"Sample package fixture was not found: {samplePackagePath}");
        return samplePackagePath;
    }

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
