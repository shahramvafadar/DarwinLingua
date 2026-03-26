using DarwinLingua.Catalog.Domain.Entities;
using DarwinLingua.Infrastructure.Persistence;
using DarwinLingua.Infrastructure.Persistence.Abstractions;
using DarwinLingua.SharedKernel.Globalization;
using Microsoft.EntityFrameworkCore;

namespace DarwinLingua.Catalog.Infrastructure.Seed;

/// <summary>
/// Seeds the stable Phase 1 topic reference data into the shared SQLite database.
/// </summary>
internal sealed class CatalogReferenceDataSeeder : IDatabaseSeeder
{
    private static readonly TopicSeedDefinition[] SeedDefinitions =
    [
        new(new Guid("6F1464F0-9807-420F-A86E-41731D11A001"), "everyday-life", 10, true,
            [new(new Guid("6F1464F0-9807-420F-A86E-41731D11B001"), "en", "Everyday Life"),
             new(new Guid("6F1464F0-9807-420F-A86E-41731D11B002"), "de", "Alltag")]),
        new(new Guid("6F1464F0-9807-420F-A86E-41731D11A002"), "housing", 20, true,
            [new(new Guid("6F1464F0-9807-420F-A86E-41731D11B003"), "en", "Housing"),
             new(new Guid("6F1464F0-9807-420F-A86E-41731D11B004"), "de", "Wohnen")]),
        new(new Guid("6F1464F0-9807-420F-A86E-41731D11A003"), "shopping", 30, true,
            [new(new Guid("6F1464F0-9807-420F-A86E-41731D11B005"), "en", "Shopping"),
             new(new Guid("6F1464F0-9807-420F-A86E-41731D11B006"), "de", "Einkaufen")]),
        new(new Guid("6F1464F0-9807-420F-A86E-41731D11A004"), "work-and-jobs", 40, true,
            [new(new Guid("6F1464F0-9807-420F-A86E-41731D11B007"), "en", "Work and Jobs"),
             new(new Guid("6F1464F0-9807-420F-A86E-41731D11B008"), "de", "Arbeit und Beruf")]),
        new(new Guid("6F1464F0-9807-420F-A86E-41731D11A005"), "appointments-and-health", 50, true,
            [new(new Guid("6F1464F0-9807-420F-A86E-41731D11B009"), "en", "Appointments and Health"),
             new(new Guid("6F1464F0-9807-420F-A86E-41731D11B010"), "de", "Termine und Gesundheit")]),
    ];

    private readonly IDbContextFactory<DarwinLinguaDbContext> _dbContextFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="CatalogReferenceDataSeeder"/> class.
    /// </summary>
    public CatalogReferenceDataSeeder(IDbContextFactory<DarwinLinguaDbContext> dbContextFactory)
    {
        ArgumentNullException.ThrowIfNull(dbContextFactory);

        _dbContextFactory = dbContextFactory;
    }

    /// <inheritdoc />
    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        await using DarwinLinguaDbContext dbContext = await _dbContextFactory
            .CreateDbContextAsync(cancellationToken)
            .ConfigureAwait(false);

        List<Topic> existingTopics = await dbContext.Topics
            .Include(topic => topic.Localizations)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        DateTime timestampUtc = DateTime.UtcNow;

        foreach (TopicSeedDefinition seedDefinition in SeedDefinitions)
        {
            Topic? existingTopic = existingTopics.SingleOrDefault(topic => topic.Key == seedDefinition.Key);

            if (existingTopic is null)
            {
                Topic newTopic = new(
                    seedDefinition.Id,
                    seedDefinition.Key,
                    seedDefinition.SortOrder,
                    seedDefinition.IsSystem,
                    timestampUtc);

                foreach (TopicLocalizationSeedDefinition localization in seedDefinition.Localizations)
                {
                    newTopic.AddOrUpdateLocalization(
                        localization.Id,
                        LanguageCode.From(localization.LanguageCode),
                        localization.DisplayName,
                        timestampUtc);
                }

                dbContext.Topics.Add(newTopic);
                continue;
            }

            existingTopic.UpdateSortOrder(seedDefinition.SortOrder, timestampUtc);

            foreach (TopicLocalizationSeedDefinition localization in seedDefinition.Localizations)
            {
                existingTopic.AddOrUpdateLocalization(
                    localization.Id,
                    LanguageCode.From(localization.LanguageCode),
                    localization.DisplayName,
                    timestampUtc);
            }
        }

        if (dbContext.ChangeTracker.HasChanges())
        {
            await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    private sealed record TopicSeedDefinition(
        Guid Id,
        string Key,
        int SortOrder,
        bool IsSystem,
        IReadOnlyList<TopicLocalizationSeedDefinition> Localizations);

    private sealed record TopicLocalizationSeedDefinition(
        Guid Id,
        string LanguageCode,
        string DisplayName);
}
