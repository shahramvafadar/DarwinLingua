namespace DarwinLingua.Practice.Application.Models;

/// <summary>
/// Represents a preview word surfaced by the practice overview.
/// </summary>
public sealed record PracticeWordPreviewModel(
    Guid WordEntryPublicId,
    string Lemma,
    string CefrLevel,
    string? PrimaryMeaning,
    bool IsKnown,
    bool IsDifficult,
    int ViewCount,
    DateTime? LastViewedAtUtc);
