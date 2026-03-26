using DarwinLingua.Infrastructure.Persistence;
using DarwinLingua.Learning.Application.Abstractions;
using DarwinLingua.Learning.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DarwinLingua.Learning.Infrastructure.Repositories;

/// <summary>
/// Reads and writes the local user's favorite words in the shared SQLite database.
/// </summary>
internal sealed class UserFavoriteWordRepository : IUserFavoriteWordRepository
{
    private readonly IDbContextFactory<DarwinLinguaDbContext> _dbContextFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserFavoriteWordRepository"/> class.
    /// </summary>
    public UserFavoriteWordRepository(IDbContextFactory<DarwinLinguaDbContext> dbContextFactory)
    {
        ArgumentNullException.ThrowIfNull(dbContextFactory);

        _dbContextFactory = dbContextFactory;
    }

    /// <inheritdoc />
    public async Task<UserFavoriteWord?> GetByUserIdAndWordPublicIdAsync(
        string userId,
        Guid wordEntryPublicId,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);

        if (wordEntryPublicId == Guid.Empty)
        {
            throw new ArgumentException("Word public identifier cannot be empty.", nameof(wordEntryPublicId));
        }

        await using DarwinLinguaDbContext dbContext = await _dbContextFactory
            .CreateDbContextAsync(cancellationToken)
            .ConfigureAwait(false);

        return await dbContext.UserFavoriteWords
            .SingleOrDefaultAsync(
                favoriteWord => favoriteWord.UserId == userId && favoriteWord.WordEntryPublicId == wordEntryPublicId,
                cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task AddAsync(UserFavoriteWord favoriteWord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(favoriteWord);

        await using DarwinLinguaDbContext dbContext = await _dbContextFactory
            .CreateDbContextAsync(cancellationToken)
            .ConfigureAwait(false);

        dbContext.UserFavoriteWords.Add(favoriteWord);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(UserFavoriteWord favoriteWord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(favoriteWord);

        await using DarwinLinguaDbContext dbContext = await _dbContextFactory
            .CreateDbContextAsync(cancellationToken)
            .ConfigureAwait(false);

        dbContext.UserFavoriteWords.Remove(favoriteWord);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
