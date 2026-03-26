using DarwinLingua.Infrastructure.Persistence;
using DarwinLingua.Localization.Application.Abstractions;
using DarwinLingua.Localization.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DarwinLingua.Localization.Infrastructure.Repositories;

/// <summary>
/// Reads localization language reference data from the shared SQLite database.
/// </summary>
internal sealed class LanguageRepository : ILanguageRepository
{
    private readonly IDbContextFactory<DarwinLinguaDbContext> _dbContextFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="LanguageRepository"/> class.
    /// </summary>
    /// <param name="dbContextFactory">The context factory used to open database sessions.</param>
    public LanguageRepository(IDbContextFactory<DarwinLinguaDbContext> dbContextFactory)
    {
        ArgumentNullException.ThrowIfNull(dbContextFactory);

        _dbContextFactory = dbContextFactory;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<Language>> GetActiveAsync(CancellationToken cancellationToken)
    {
        await using DarwinLinguaDbContext dbContext = await _dbContextFactory
            .CreateDbContextAsync(cancellationToken)
            .ConfigureAwait(false);

        return await dbContext.Languages
            .AsNoTracking()
            .Where(language => language.IsActive)
            .OrderBy(language => language.EnglishName)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}
