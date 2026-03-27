using DarwinLingua.SharedKernel.Exceptions;

namespace DarwinLingua.Practice.Domain.Entities;

/// <summary>
/// Represents the persisted learner review state and scheduling snapshot for a lexical entry.
/// </summary>
public sealed class PracticeReviewState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PracticeReviewState"/> class for EF Core materialization.
    /// </summary>
    private PracticeReviewState()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PracticeReviewState"/> class.
    /// </summary>
    public PracticeReviewState(Guid id, string userId, Guid wordEntryPublicId, DateTime createdAtUtc)
    {
        if (id == Guid.Empty)
        {
            throw new DomainRuleException("Practice-review-state identifier cannot be empty.");
        }

        if (wordEntryPublicId == Guid.Empty)
        {
            throw new DomainRuleException("Practice-review-state lexical entry identifier cannot be empty.");
        }

        Id = id;
        UserId = NormalizeRequiredText(userId, nameof(userId));
        WordEntryPublicId = wordEntryPublicId;
        CreatedAtUtc = NormalizeUtc(createdAtUtc, nameof(createdAtUtc));
        UpdatedAtUtc = CreatedAtUtc;
    }

    /// <summary>
    /// Gets the stable internal identifier of the review-state row.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the stable local user identifier.
    /// </summary>
    public string UserId { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the public lexical-entry identifier referenced by the review-state row.
    /// </summary>
    public Guid WordEntryPublicId { get; private set; }

    /// <summary>
    /// Gets the next scheduled due timestamp, when one exists.
    /// </summary>
    public DateTime? DueAtUtc { get; private set; }

    /// <summary>
    /// Gets the most recent attempt timestamp.
    /// </summary>
    public DateTime? LastAttemptedAtUtc { get; private set; }

    /// <summary>
    /// Gets the most recent successful attempt timestamp.
    /// </summary>
    public DateTime? LastSuccessfulAttemptedAtUtc { get; private set; }

    /// <summary>
    /// Gets the most recent session type used for this word.
    /// </summary>
    public PracticeSessionType? LastSessionType { get; private set; }

    /// <summary>
    /// Gets the most recent attempt outcome.
    /// </summary>
    public PracticeAttemptOutcome? LastOutcome { get; private set; }

    /// <summary>
    /// Gets the number of consecutive successful attempts.
    /// </summary>
    public int ConsecutiveSuccessCount { get; private set; }

    /// <summary>
    /// Gets the number of consecutive unsuccessful attempts.
    /// </summary>
    public int ConsecutiveFailureCount { get; private set; }

    /// <summary>
    /// Gets the total number of persisted attempts.
    /// </summary>
    public int TotalAttemptCount { get; private set; }

    /// <summary>
    /// Gets the UTC creation timestamp.
    /// </summary>
    public DateTime CreatedAtUtc { get; private set; }

    /// <summary>
    /// Gets the UTC last update timestamp.
    /// </summary>
    public DateTime UpdatedAtUtc { get; private set; }

    /// <summary>
    /// Updates the persisted scheduling snapshot after a learner attempt.
    /// </summary>
    public void RecordAttempt(
        PracticeSessionType sessionType,
        PracticeAttemptOutcome outcome,
        DateTime attemptedAtUtc,
        DateTime? dueAtUtcAfterAttempt)
    {
        DateTime normalizedAttemptedAtUtc = NormalizeUtc(attemptedAtUtc, nameof(attemptedAtUtc));

        LastSessionType = sessionType;
        LastOutcome = outcome;
        LastAttemptedAtUtc = normalizedAttemptedAtUtc;
        DueAtUtc = NormalizeOptionalUtc(dueAtUtcAfterAttempt);
        TotalAttemptCount++;

        if (outcome == PracticeAttemptOutcome.Incorrect)
        {
            ConsecutiveFailureCount++;
            ConsecutiveSuccessCount = 0;
        }
        else
        {
            ConsecutiveSuccessCount++;
            ConsecutiveFailureCount = 0;
            LastSuccessfulAttemptedAtUtc = normalizedAttemptedAtUtc;
        }

        UpdatedAtUtc = normalizedAttemptedAtUtc;
    }

    /// <summary>
    /// Seeds or adjusts the next due timestamp without recording a learner answer.
    /// </summary>
    public void SetDueAt(DateTime? dueAtUtc, DateTime updatedAtUtc)
    {
        DueAtUtc = NormalizeOptionalUtc(dueAtUtc);
        UpdatedAtUtc = NormalizeUtc(updatedAtUtc, nameof(updatedAtUtc));
    }

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
