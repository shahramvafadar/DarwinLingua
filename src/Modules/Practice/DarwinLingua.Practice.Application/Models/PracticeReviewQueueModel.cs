namespace DarwinLingua.Practice.Application.Models;

/// <summary>
/// Represents the learner's current ordered review queue.
/// </summary>
public sealed record PracticeReviewQueueModel(
    int TotalCandidates,
    IReadOnlyList<PracticeReviewQueueItemModel> Items);
