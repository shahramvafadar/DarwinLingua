using DarwinLingua.Practice.Domain.Entities;

namespace DarwinLingua.Practice.Application.Models;

/// <summary>
/// Represents one item in a started review-session snapshot.
/// </summary>
public sealed record PracticeReviewSessionItemModel(
    int Position,
    Guid WordEntryPublicId,
    string Lemma,
    string CefrLevel,
    string? PrimaryMeaning,
    DateTime? DueAtUtc,
    bool IsDueNow,
    bool IsDifficult,
    bool IsKnown,
    int ViewCount,
    DateTime LastViewedAtUtc,
    PracticeAttemptOutcome? LastOutcome,
    int TotalAttemptCount);
