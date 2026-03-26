using DarwinLingua.Infrastructure.Persistence;
using DarwinLingua.Learning.Application.Abstractions;
using DarwinLingua.Learning.Application.Models;
using DarwinLingua.SharedKernel.Content;
using DarwinLingua.SharedKernel.Globalization;
using Microsoft.EntityFrameworkCore;

namespace DarwinLingua.Learning.Infrastructure.Services;

/// <summary>
/// Reads lexical catalog data required for favorite-word workflows.
/// </summary>
internal sealed class UserFavoriteWordCatalogReader : IUserFavoriteWordCatalogReader
{
    private readonly IDbContextFactory<DarwinLinguaDbContext> _dbContextFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserFavoriteWordCatalogReader"/> class.
    /// </summary>
    public UserFavoriteWordCatalogReader(IDbContextFactory<DarwinLinguaDbContext> dbContextFactory)
    {
        ArgumentNullException.ThrowIfNull(dbContextFactory);

        _dbContextFactory = dbContextFactory;
    }

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(Guid wordEntryPublicId, CancellationToken cancellationToken)
    {
        if (wordEntryPublicId == Guid.Empty)
        {
            throw new ArgumentException("Word public identifier cannot be empty.", nameof(wordEntryPublicId));
        }

        await using DarwinLinguaDbContext dbContext = await _dbContextFactory
            .CreateDbContextAsync(cancellationToken)
            .ConfigureAwait(false);

        return await dbContext.WordEntries
            .AsNoTracking()
            .AnyAsync(
                word => word.PublicId == wordEntryPublicId && word.PublicationStatus == PublicationStatus.Active,
                cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<FavoriteWordListItemModel>> GetFavoriteWordsAsync(
        string userId,
        string meaningLanguageCode,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        ArgumentException.ThrowIfNullOrWhiteSpace(meaningLanguageCode);

        LanguageCode preferredMeaningLanguage = LanguageCode.From(meaningLanguageCode);

        await using DarwinLinguaDbContext dbContext = await _dbContextFactory
            .CreateDbContextAsync(cancellationToken)
            .ConfigureAwait(false);

        FavoriteWordProjection[] favoriteWords = await dbContext.UserFavoriteWords
            .AsNoTracking()
            .Where(favoriteWord => favoriteWord.UserId == userId)
            .OrderByDescending(favoriteWord => favoriteWord.CreatedAtUtc)
            .Select(favoriteWord => new FavoriteWordProjection(
                favoriteWord.WordEntryPublicId,
                favoriteWord.CreatedAtUtc))
            .ToArrayAsync(cancellationToken)
            .ConfigureAwait(false);

        if (favoriteWords.Length == 0)
        {
            return [];
        }

        Guid[] favoritePublicIds = favoriteWords.Select(favoriteWord => favoriteWord.WordEntryPublicId).ToArray();

        Dictionary<Guid, FavoriteWordListItemModel> lexicalEntries = await dbContext.WordEntries
            .AsNoTracking()
            .Where(word => word.PublicationStatus == PublicationStatus.Active)
            .Where(word => favoritePublicIds.Contains(word.PublicId))
            .Include(word => word.Senses)
                .ThenInclude(sense => sense.Translations)
            .ToDictionaryAsync(
                word => word.PublicId,
                word => Map(word, preferredMeaningLanguage),
                cancellationToken)
            .ConfigureAwait(false);

        return favoriteWords
            .Where(favoriteWord => lexicalEntries.ContainsKey(favoriteWord.WordEntryPublicId))
            .Select(favoriteWord => lexicalEntries[favoriteWord.WordEntryPublicId])
            .ToArray();
    }

    /// <summary>
    /// Maps the lexical aggregate to a favorite-word summary model.
    /// </summary>
    private static FavoriteWordListItemModel Map(Catalog.Domain.Entities.WordEntry word, LanguageCode preferredMeaningLanguage)
    {
        ArgumentNullException.ThrowIfNull(word);

        string? primaryMeaning = word.GetPrimarySense()?
            .Translations
            .FirstOrDefault(translation => translation.LanguageCode == preferredMeaningLanguage)?
            .TranslationText;

        return new FavoriteWordListItemModel(
            word.PublicId,
            word.Lemma,
            word.Article,
            word.PartOfSpeech.ToString(),
            word.PrimaryCefrLevel.ToString(),
            primaryMeaning);
    }

    /// <summary>
    /// Represents the persisted sort context for favorite-word result composition.
    /// </summary>
    private sealed record FavoriteWordProjection(Guid WordEntryPublicId, DateTime CreatedAtUtc);
}
