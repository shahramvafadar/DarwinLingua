using DarwinLingua.SharedKernel.Exceptions;

namespace DarwinLingua.Learning.Domain.Entities;

/// <summary>
/// Represents a user-owned favorite marker for a lexical entry.
/// </summary>
public sealed class UserFavoriteWord
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserFavoriteWord"/> class for EF Core materialization.
    /// </summary>
    private UserFavoriteWord()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserFavoriteWord"/> class.
    /// </summary>
    public UserFavoriteWord(Guid id, string userId, Guid wordEntryPublicId, DateTime createdAtUtc)
    {
        if (id == Guid.Empty)
        {
            throw new DomainRuleException("Favorite-word identifier cannot be empty.");
        }

        if (wordEntryPublicId == Guid.Empty)
        {
            throw new DomainRuleException("Favorite-word lexical entry identifier cannot be empty.");
        }

        Id = id;
        UserId = NormalizeRequiredText(userId, nameof(userId));
        WordEntryPublicId = wordEntryPublicId;
        CreatedAtUtc = NormalizeUtc(createdAtUtc, nameof(createdAtUtc));
    }

    /// <summary>
    /// Gets the stable internal identifier of the favorite row.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the stable local user identifier.
    /// </summary>
    public string UserId { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the public lexical-entry identifier referenced by the favorite row.
    /// </summary>
    public Guid WordEntryPublicId { get; private set; }

    /// <summary>
    /// Gets the UTC creation timestamp.
    /// </summary>
    public DateTime CreatedAtUtc { get; private set; }

    private static string NormalizeRequiredText(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainRuleException($"{parameterName} cannot be empty.");
        }

        return value.Trim();
    }

    private static DateTime NormalizeUtc(DateTime value, string parameterName)
    {
        if (value == default)
        {
            throw new DomainRuleException($"{parameterName} cannot be empty.");
        }

        return value.Kind == DateTimeKind.Utc ? value : value.ToUniversalTime();
    }
}
