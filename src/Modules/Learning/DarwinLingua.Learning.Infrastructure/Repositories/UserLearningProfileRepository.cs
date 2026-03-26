using DarwinLingua.Infrastructure.Persistence;
using DarwinLingua.Learning.Application.Abstractions;
using DarwinLingua.Learning.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DarwinLingua.Learning.Infrastructure.Repositories;

/// <summary>
/// Reads and writes the local user learning profile in the shared SQLite database.
/// </summary>
internal sealed class UserLearningProfileRepository : IUserLearningProfileRepository
{
    private readonly IDbContextFactory<DarwinLinguaDbContext> _dbContextFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserLearningProfileRepository"/> class.
    /// </summary>
    public UserLearningProfileRepository(IDbContextFactory<DarwinLinguaDbContext> dbContextFactory)
    {
        ArgumentNullException.ThrowIfNull(dbContextFactory);

        _dbContextFactory = dbContextFactory;
    }

    /// <inheritdoc />
    public async Task<UserLearningProfile?> GetByUserIdAsync(string userId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);

        await using DarwinLinguaDbContext dbContext = await _dbContextFactory
            .CreateDbContextAsync(cancellationToken)
            .ConfigureAwait(false);

        return await dbContext.UserLearningProfiles
            .AsNoTracking()
            .SingleOrDefaultAsync(profile => profile.UserId == userId, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task AddAsync(UserLearningProfile profile, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(profile);

        await using DarwinLinguaDbContext dbContext = await _dbContextFactory
            .CreateDbContextAsync(cancellationToken)
            .ConfigureAwait(false);

        dbContext.UserLearningProfiles.Add(profile);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(UserLearningProfile profile, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(profile);

        await using DarwinLinguaDbContext dbContext = await _dbContextFactory
            .CreateDbContextAsync(cancellationToken)
            .ConfigureAwait(false);

        dbContext.UserLearningProfiles.Update(profile);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
