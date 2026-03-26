using DarwinLingua.Catalog.Application.Abstractions;
using DarwinLingua.Catalog.Domain.Entities;
using DarwinLingua.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DarwinLingua.Catalog.Infrastructure.Repositories;

/// <summary>
/// Reads topic aggregates from the shared SQLite database.
/// </summary>
internal sealed class TopicRepository : ITopicRepository
{
    private readonly IDbContextFactory<DarwinLinguaDbContext> _dbContextFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="TopicRepository"/> class.
    /// </summary>
    public TopicRepository(IDbContextFactory<DarwinLinguaDbContext> dbContextFactory)
    {
        ArgumentNullException.ThrowIfNull(dbContextFactory);

        _dbContextFactory = dbContextFactory;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<Topic>> GetAllAsync(CancellationToken cancellationToken)
    {
        await using DarwinLinguaDbContext dbContext = await _dbContextFactory
            .CreateDbContextAsync(cancellationToken)
            .ConfigureAwait(false);

        return await dbContext.Topics
            .AsNoTracking()
            .Include(topic => topic.Localizations)
            .OrderBy(topic => topic.SortOrder)
            .ThenBy(topic => topic.Key)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}
