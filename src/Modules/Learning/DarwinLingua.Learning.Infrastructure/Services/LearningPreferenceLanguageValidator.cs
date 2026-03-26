using DarwinLingua.Infrastructure.Persistence;
using DarwinLingua.Learning.Application.Abstractions;
using DarwinLingua.SharedKernel.Globalization;
using Microsoft.EntityFrameworkCore;

namespace DarwinLingua.Learning.Infrastructure.Services;

/// <summary>
/// Validates whether language codes are available for local user preference scenarios.
/// </summary>
internal sealed class LearningPreferenceLanguageValidator : ILearningPreferenceLanguageValidator
{
    private readonly IDbContextFactory<DarwinLinguaDbContext> _dbContextFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="LearningPreferenceLanguageValidator"/> class.
    /// </summary>
    public LearningPreferenceLanguageValidator(IDbContextFactory<DarwinLinguaDbContext> dbContextFactory)
    {
        ArgumentNullException.ThrowIfNull(dbContextFactory);

        _dbContextFactory = dbContextFactory;
    }

    /// <inheritdoc />
    public async Task<bool> SupportsUserInterfaceAsync(LanguageCode languageCode, CancellationToken cancellationToken)
    {
        await using DarwinLinguaDbContext dbContext = await _dbContextFactory
            .CreateDbContextAsync(cancellationToken)
            .ConfigureAwait(false);

        return await dbContext.Languages
            .AsNoTracking()
            .AnyAsync(
                language => language.IsActive &&
                    language.SupportsUserInterface &&
                    language.Code == languageCode,
                cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<bool> SupportsMeaningsAsync(LanguageCode languageCode, CancellationToken cancellationToken)
    {
        await using DarwinLinguaDbContext dbContext = await _dbContextFactory
            .CreateDbContextAsync(cancellationToken)
            .ConfigureAwait(false);

        return await dbContext.Languages
            .AsNoTracking()
            .AnyAsync(
                language => language.IsActive &&
                    language.SupportsMeanings &&
                    language.Code == languageCode,
                cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<LanguageCode>> GetSupportedMeaningLanguagesAsync(CancellationToken cancellationToken)
    {
        await using DarwinLinguaDbContext dbContext = await _dbContextFactory
            .CreateDbContextAsync(cancellationToken)
            .ConfigureAwait(false);

        return await dbContext.Languages
            .AsNoTracking()
            .Where(language => language.IsActive && language.SupportsMeanings)
            .OrderBy(language => language.EnglishName)
            .Select(language => language.Code)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}
