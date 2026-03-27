using DarwinLingua.Practice.Domain.Entities;

namespace DarwinLingua.Practice.Application.Models;

/// <summary>
/// Represents the persisted quiz-answer result and updated scheduling snapshot.
/// </summary>
public sealed record PracticeQuizAnswerResultModel(
    Guid WordEntryPublicId,
    PracticeAttemptOutcome Outcome,
    DateTime AttemptedAtUtc,
    DateTime? DueAtUtcBeforeAttempt,
    DateTime DueAtUtcAfterAttempt,
    int TotalAttemptCount,
    int ConsecutiveSuccessCount,
    int ConsecutiveFailureCount);
