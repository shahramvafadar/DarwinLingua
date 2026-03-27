using DarwinLingua.SharedKernel.Exceptions;

namespace DarwinLingua.Practice.Domain.Entities;

/// <summary>
/// Represents one persisted learner practice attempt for a lexical entry.
/// </summary>
public sealed class PracticeAttempt
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PracticeAttempt"/> class for EF Core materialization.
    /// </summary>
    private PracticeAttempt()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PracticeAttempt"/> class.
    /// </summary>
    public PracticeAttempt(
        Guid id,
        string userId,
        Guid wordEntryPublicId,
        PracticeSessionType sessionType,
        PracticeAttemptOutcome outcome,
        DateTime attemptedAtUtc,
        DateTime? dueAtUtcBeforeAttempt = null,
        DateTime? dueAtUtcAfterAttempt = null,
        int? responseMilliseconds = null)
    {
        if (id == Guid.Empty)
        {
            throw new DomainRuleException("Practice-attempt identifier cannot be empty.");
        }

        if (wordEntryPublicId == Guid.Empty)
        {
            throw new DomainRuleException("Practice-attempt lexical entry identifier cannot be empty.");
        }

        if (responseMilliseconds is <= 0)
        {
            throw new DomainRuleException("Response milliseconds must be greater than zero when provided.");
        }

        Id = id;
        UserId = NormalizeRequiredText(userId, nameof(userId));
        WordEntryPublicId = wordEntryPublicId;
        SessionType = sessionType;
        Outcome = outcome;
        DueAtUtcBeforeAttempt = NormalizeOptionalUtc(dueAtUtcBeforeAttempt);
        DueAtUtcAfterAttempt = NormalizeOptionalUtc(dueAtUtcAfterAttempt);
        AttemptedAtUtc = NormalizeUtc(attemptedAtUtc, nameof(attemptedAtUtc));
        ResponseMilliseconds = responseMilliseconds;
        CreatedAtUtc = AttemptedAtUtc;
    }

    /// <summary>
    /// Gets the stable internal identifier of the attempt row.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the stable local user identifier.
    /// </summary>
    public string UserId { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the public lexical-entry identifier referenced by the attempt row.
    /// </summary>
    public Guid WordEntryPublicId { get; private set; }

    /// <summary>
    /// Gets the learner session type that produced this attempt.
    /// </summary>
    public PracticeSessionType SessionType { get; private set; }

    /// <summary>
    /// Gets the evaluated attempt outcome.
    /// </summary>
    public PracticeAttemptOutcome Outcome { get; private set; }

    /// <summary>
    /// Gets the scheduled review due timestamp that existed before this attempt.
    /// </summary>
    public DateTime? DueAtUtcBeforeAttempt { get; private set; }

    /// <summary>
    /// Gets the scheduled review due timestamp computed after this attempt.
    /// </summary>
    public DateTime? DueAtUtcAfterAttempt { get; private set; }

    /// <summary>
    /// Gets the UTC timestamp when the learner submitted the attempt.
    /// </summary>
    public DateTime AttemptedAtUtc { get; private set; }

    /// <summary>
    /// Gets the optional answer latency in milliseconds.
    /// </summary>
    public int? ResponseMilliseconds { get; private set; }

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

    private static DateTime? NormalizeOptionalUtc(DateTime? value)
    {
        return value is null ? null : NormalizeUtc(value.Value, nameof(value));
    }
}
