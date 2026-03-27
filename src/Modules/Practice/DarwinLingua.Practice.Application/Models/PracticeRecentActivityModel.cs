namespace DarwinLingua.Practice.Application.Models;

/// <summary>
/// Represents the learner's recent persisted practice activity.
/// </summary>
public sealed record PracticeRecentActivityModel(
    int TotalAttempts,
    IReadOnlyList<PracticeRecentActivityItemModel> Items);
