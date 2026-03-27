namespace DarwinLingua.Practice.Application.Models;

/// <summary>
/// Represents the local learner's current practice and progress snapshot.
/// </summary>
public sealed record PracticeOverviewModel(
    int TotalTrackedWords,
    int ReviewCandidateCount,
    int DifficultWordCount,
    int KnownWordCount,
    int RecentlyViewedCount,
    DateTime? LastActivityAtUtc,
    IReadOnlyList<PracticeWordPreviewModel> ReviewPreview);
