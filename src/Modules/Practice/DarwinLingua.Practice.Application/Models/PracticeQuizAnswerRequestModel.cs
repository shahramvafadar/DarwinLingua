using DarwinLingua.Practice.Domain.Entities;

namespace DarwinLingua.Practice.Application.Models;

/// <summary>
/// Represents one quiz answer submitted by the learner.
/// </summary>
public sealed record PracticeQuizAnswerRequestModel(
    Guid WordEntryPublicId,
    PracticeAttemptOutcome Outcome,
    int? ResponseMilliseconds = null,
    DateTime? AttemptedAtUtc = null);
