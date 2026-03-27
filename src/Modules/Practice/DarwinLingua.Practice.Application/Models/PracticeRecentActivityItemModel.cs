using DarwinLingua.Practice.Domain.Entities;

namespace DarwinLingua.Practice.Application.Models;

/// <summary>
/// Represents one recent persisted learner practice attempt.
/// </summary>
public sealed record PracticeRecentActivityItemModel(
    Guid WordEntryPublicId,
    string Lemma,
    string CefrLevel,
    string? PrimaryMeaning,
    PracticeSessionType SessionType,
    PracticeAttemptOutcome Outcome,
    DateTime AttemptedAtUtc,
    DateTime? DueAtUtcAfterAttempt,
    int? ResponseMilliseconds);
