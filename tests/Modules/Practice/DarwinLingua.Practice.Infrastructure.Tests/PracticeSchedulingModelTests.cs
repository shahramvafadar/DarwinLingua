using DarwinLingua.Practice.Domain.Entities;
using DarwinLingua.SharedKernel.Exceptions;

namespace DarwinLingua.Practice.Infrastructure.Tests;

/// <summary>
/// Verifies core Practice scheduling-domain invariants and state transitions.
/// </summary>
public sealed class PracticeSchedulingModelTests
{
    /// <summary>
    /// Verifies that review state tracks success and failure streaks consistently.
    /// </summary>
    [Fact]
    public void PracticeReviewState_RecordAttempt_ShouldUpdateCountersAndDueDate()
    {
        PracticeReviewState reviewState = new(
            Guid.NewGuid(),
            "local-installation-user",
            Guid.NewGuid(),
            DateTime.UtcNow.AddMinutes(-5));

        DateTime firstAttemptAtUtc = DateTime.UtcNow.AddMinutes(-2);
        DateTime secondAttemptAtUtc = DateTime.UtcNow.AddMinutes(-1);

        reviewState.RecordAttempt(
            PracticeSessionType.Flashcard,
            PracticeAttemptOutcome.Correct,
            firstAttemptAtUtc,
            firstAttemptAtUtc.AddHours(1));

        reviewState.RecordAttempt(
            PracticeSessionType.Quiz,
            PracticeAttemptOutcome.Incorrect,
            secondAttemptAtUtc,
            secondAttemptAtUtc.AddMinutes(10));

        Assert.Equal(2, reviewState.TotalAttemptCount);
        Assert.Equal(0, reviewState.ConsecutiveSuccessCount);
        Assert.Equal(1, reviewState.ConsecutiveFailureCount);
        Assert.Equal(PracticeSessionType.Quiz, reviewState.LastSessionType);
        Assert.Equal(PracticeAttemptOutcome.Incorrect, reviewState.LastOutcome);
        Assert.Equal(secondAttemptAtUtc.AddMinutes(10), reviewState.DueAtUtc);
        Assert.Equal(firstAttemptAtUtc, reviewState.LastSuccessfulAttemptedAtUtc);
    }

    /// <summary>
    /// Verifies that practice attempts reject invalid response-duration values.
    /// </summary>
    [Fact]
    public void PracticeAttempt_ShouldRejectNonPositiveResponseMilliseconds()
    {
        Assert.Throws<DomainRuleException>(() => new PracticeAttempt(
            Guid.NewGuid(),
            "local-installation-user",
            Guid.NewGuid(),
            PracticeSessionType.Review,
            PracticeAttemptOutcome.Hard,
            DateTime.UtcNow,
            responseMilliseconds: 0));
    }
}
