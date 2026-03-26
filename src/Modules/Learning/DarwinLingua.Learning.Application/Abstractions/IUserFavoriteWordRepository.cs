using DarwinLingua.Learning.Domain.Entities;

namespace DarwinLingua.Learning.Application.Abstractions;

/// <summary>
/// Provides persistence operations for local user favorite words.
/// </summary>
public interface IUserFavoriteWordRepository
{
    /// <summary>
    /// Loads a favorite row for the specified user and lexical entry.
    /// </summary>
    Task<UserFavoriteWord?> GetByUserIdAndWordPublicIdAsync(
        string userId,
        Guid wordEntryPublicId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Persists a newly created favorite row.
    /// </summary>
    Task AddAsync(UserFavoriteWord favoriteWord, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes an existing favorite row.
    /// </summary>
    Task DeleteAsync(UserFavoriteWord favoriteWord, CancellationToken cancellationToken);
}
