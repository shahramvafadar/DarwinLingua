using DarwinLingua.Infrastructure.Persistence;
using DarwinLingua.Practice.Application.Abstractions;
using DarwinLingua.Practice.Application.Models;
using DarwinLingua.SharedKernel.Content;
using DarwinLingua.SharedKernel.Globalization;
using Microsoft.EntityFrameworkCore;

namespace DarwinLingua.Practice.Infrastructure.Queries;

/// <summary>
/// Reads practice-overview projections from the shared local SQLite database.
/// </summary>
internal sealed class PracticeOverviewReader : IPracticeOverviewReader
{
    private const int PreviewSize = 5;

    private readonly IDbContextFactory<DarwinLinguaDbContext> _dbContextFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="PracticeOverviewReader"/> class.
    /// </summary>
    public PracticeOverviewReader(IDbContextFactory<DarwinLinguaDbContext> dbContextFactory)
    {
        ArgumentNullException.ThrowIfNull(dbContextFactory);

        _dbContextFactory = dbContextFactory;
    }

    /// <inheritdoc />
    public async Task<PracticeOverviewModel> GetOverviewAsync(LanguageCode meaningLanguageCode, CancellationToken cancellationToken)
    {
        await using DarwinLinguaDbContext dbContext = await _dbContextFactory
            .CreateDbContextAsync(cancellationToken)
            .ConfigureAwait(false);

        List<PracticeStateRow> trackedRows = await (
            from userWordState in dbContext.UserWordStates.AsNoTracking()
            join wordEntry in dbContext.WordEntries.AsNoTracking()
                on userWordState.WordEntryPublicId equals wordEntry.PublicId
            where userWordState.UserId == LocalInstallationUser.UserId &&
                wordEntry.PublicationStatus == PublicationStatus.Active
            select new PracticeStateRow(
                wordEntry.Id,
                wordEntry.PublicId,
                wordEntry.Lemma,
                wordEntry.PrimaryCefrLevel.ToString(),
                userWordState.IsKnown,
                userWordState.IsDifficult,
                userWordState.ViewCount,
                userWordState.LastViewedAtUtc,
                userWordState.UpdatedAtUtc))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        List<PracticeStateRow> reviewCandidates = trackedRows
            .Where(row => row.LastViewedAtUtc is not null && (!row.IsKnown || row.IsDifficult))
            .OrderByDescending(row => row.IsDifficult)
            .ThenBy(row => row.LastViewedAtUtc)
            .ThenByDescending(row => row.ViewCount)
            .Take(PreviewSize)
            .ToList();

        Dictionary<Guid, string?> meaningsByWordEntryId = await LoadMeaningsByWordEntryIdAsync(
            dbContext,
            reviewCandidates.Select(row => row.WordEntryId).Distinct().ToArray(),
            meaningLanguageCode,
            cancellationToken).ConfigureAwait(false);

        return new PracticeOverviewModel(
            trackedRows.Count,
            trackedRows.Count(row => row.LastViewedAtUtc is not null && (!row.IsKnown || row.IsDifficult)),
            trackedRows.Count(row => row.IsDifficult),
            trackedRows.Count(row => row.IsKnown),
            trackedRows.Count(row => row.LastViewedAtUtc is not null),
            trackedRows
                .OrderByDescending(row => row.UpdatedAtUtc)
                .Select(row => (DateTime?)row.UpdatedAtUtc)
                .FirstOrDefault(),
            reviewCandidates
                .Select(row => new PracticeWordPreviewModel(
                    row.WordEntryPublicId,
                    row.Lemma,
                    row.CefrLevel,
                    meaningsByWordEntryId.GetValueOrDefault(row.WordEntryId),
                    row.IsKnown,
                    row.IsDifficult,
                    row.ViewCount,
                    row.LastViewedAtUtc))
                .ToArray());
    }

    private static async Task<Dictionary<Guid, string?>> LoadMeaningsByWordEntryIdAsync(
        DarwinLinguaDbContext dbContext,
        Guid[] wordEntryIds,
        LanguageCode meaningLanguageCode,
        CancellationToken cancellationToken)
    {
        if (wordEntryIds.Length == 0)
        {
            return [];
        }

        List<MeaningRow> meaningRows = await (
            from wordSense in dbContext.WordSenses.AsNoTracking()
            join translation in dbContext.SenseTranslations.AsNoTracking()
                on wordSense.Id equals translation.WordSenseId
            where wordEntryIds.Contains(wordSense.WordEntryId) &&
                wordSense.PublicationStatus == PublicationStatus.Active &&
                translation.LanguageCode == meaningLanguageCode
            orderby wordSense.WordEntryId,
                wordSense.IsPrimarySense descending,
                wordSense.SenseOrder,
                translation.IsPrimary descending,
                translation.TranslationText
            select new MeaningRow(wordSense.WordEntryId, translation.TranslationText))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return meaningRows
            .GroupBy(row => row.WordEntryId)
            .ToDictionary(group => group.Key, group => (string?)group.First().TranslationText);
    }

    private sealed record PracticeStateRow(
        Guid WordEntryId,
        Guid WordEntryPublicId,
        string Lemma,
        string CefrLevel,
        bool IsKnown,
        bool IsDifficult,
        int ViewCount,
        DateTime? LastViewedAtUtc,
        DateTime UpdatedAtUtc);

    private sealed record MeaningRow(Guid WordEntryId, string TranslationText);
}
