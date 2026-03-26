using DarwinLingua.Learning.Domain.Entities;

namespace DarwinLingua.Learning.Application.Abstractions;

/// <summary>
/// Provides persistence operations for the local user learning profile.
/// </summary>
public interface IUserLearningProfileRepository
{
    /// <summary>
    /// Loads the learning profile for the specified user identifier.
    /// </summary>
    Task<UserLearningProfile?> GetByUserIdAsync(string userId, CancellationToken cancellationToken);

    /// <summary>
    /// Persists a newly created learning profile.
    /// </summary>
    Task AddAsync(UserLearningProfile profile, CancellationToken cancellationToken);

    /// <summary>
    /// Persists updates made to an existing learning profile.
    /// </summary>
    Task UpdateAsync(UserLearningProfile profile, CancellationToken cancellationToken);
}
