namespace DarwinLingua.Practice.Application.Models;

/// <summary>
/// Represents a started review-session snapshot from the current review queue.
/// </summary>
public sealed record PracticeReviewSessionModel(
    DateTime StartedAtUtc,
    int TotalCandidates,
    int RequestedItemCount,
    IReadOnlyList<PracticeReviewSessionItemModel> Items);
