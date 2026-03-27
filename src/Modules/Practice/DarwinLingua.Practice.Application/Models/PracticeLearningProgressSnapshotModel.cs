namespace DarwinLingua.Practice.Application.Models;

/// <summary>
/// Represents the learner's current aggregate practice progress snapshot.
/// </summary>
public sealed record PracticeLearningProgressSnapshotModel(
    int TrackedWordCount,
    int AttemptedWordCount,
    int ScheduledWordCount,
    int DueNowCount,
    int MasteredWordCount,
    int StrugglingWordCount,
    int TotalAttemptCount,
    int CorrectAttemptCount,
    int IncorrectAttemptCount,
    double SuccessRate,
    DateTime? LastAttemptedAtUtc);
