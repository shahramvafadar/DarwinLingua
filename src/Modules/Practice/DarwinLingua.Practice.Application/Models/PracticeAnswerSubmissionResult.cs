using DarwinLingua.Practice.Domain.Entities;

namespace DarwinLingua.Practice.Application.Models;

/// <summary>
/// Represents the normalized persisted answer-submission result shared by practice modes.
/// </summary>
internal sealed record PracticeAnswerSubmissionResult(
    Guid WordEntryPublicId,
    PracticeAttemptOutcome Outcome,
    DateTime AttemptedAtUtc,
    DateTime? DueAtUtcBeforeAttempt,
    DateTime DueAtUtcAfterAttempt,
    int TotalAttemptCount,
    int ConsecutiveSuccessCount,
    int ConsecutiveFailureCount);
