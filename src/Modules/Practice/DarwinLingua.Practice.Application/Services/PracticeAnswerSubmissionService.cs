using DarwinLingua.Infrastructure.Persistence;
using DarwinLingua.Infrastructure.Persistence.Abstractions;
using DarwinLingua.Practice.Application.Models;
using DarwinLingua.Practice.Domain.Entities;
using DarwinLingua.SharedKernel.Content;
using DarwinLingua.SharedKernel.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DarwinLingua.Practice.Application.Services;

/// <summary>
/// Provides shared answer-submission logic for practice modes that update the same scheduling model.
/// </summary>
internal sealed class PracticeAnswerSubmissionService
{
    private const string LocalUserId = "local-installation-user";

    private readonly ITransactionalExecutionService _transactionalExecutionService;

    /// <summary>
    /// Initializes a new instance of the <see cref="PracticeAnswerSubmissionService"/> class.
    /// </summary>
    public PracticeAnswerSubmissionService(ITransactionalExecutionService transactionalExecutionService)
    {
        ArgumentNullException.ThrowIfNull(transactionalExecutionService);

        _transactionalExecutionService = transactionalExecutionService;
    }

    /// <summary>
    /// Persists one learner answer for the requested practice mode.
    /// </summary>
    public Task<PracticeAnswerSubmissionResult> SubmitAsync(
        Guid wordEntryPublicId,
        PracticeSessionType sessionType,
        PracticeAttemptOutcome outcome,
        int? responseMilliseconds,
        DateTime? attemptedAtUtc,
        CancellationToken cancellationToken)
    {
        if (wordEntryPublicId == Guid.Empty)
        {
            throw new ArgumentException("Word public identifier cannot be empty.", nameof(wordEntryPublicId));
        }

        if (!Enum.IsDefined(outcome))
        {
            throw new ArgumentOutOfRangeException(nameof(outcome), outcome, "Unsupported practice attempt outcome.");
        }

        DateTime resolvedAttemptedAtUtc = attemptedAtUtc ?? DateTime.UtcNow;
        resolvedAttemptedAtUtc = resolvedAttemptedAtUtc.Kind == DateTimeKind.Utc
            ? resolvedAttemptedAtUtc
            : resolvedAttemptedAtUtc.ToUniversalTime();

        return _transactionalExecutionService.ExecuteAsync(
            async (dbContext, operationCancellationToken) =>
                await SubmitAsync(
                    dbContext,
                    wordEntryPublicId,
                    sessionType,
                    outcome,
                    responseMilliseconds,
                    resolvedAttemptedAtUtc,
                    operationCancellationToken).ConfigureAwait(false),
            cancellationToken);
    }

    private static async Task<PracticeAnswerSubmissionResult> SubmitAsync(
        DarwinLinguaDbContext dbContext,
        Guid wordEntryPublicId,
        PracticeSessionType sessionType,
        PracticeAttemptOutcome outcome,
        int? responseMilliseconds,
        DateTime attemptedAtUtc,
        CancellationToken cancellationToken)
    {
        if (!await IsTrackedActiveWordAsync(dbContext, wordEntryPublicId, cancellationToken).ConfigureAwait(false))
        {
            throw new DomainRuleException("Practice answers can only be recorded for active tracked lexical entries.");
        }

        PracticeReviewState? reviewState = await dbContext.PracticeReviewStates
            .SingleOrDefaultAsync(
                state => state.UserId == LocalUserId && state.WordEntryPublicId == wordEntryPublicId,
                cancellationToken)
            .ConfigureAwait(false);

        reviewState ??= new PracticeReviewState(
            Guid.NewGuid(),
            LocalUserId,
            wordEntryPublicId,
            attemptedAtUtc);

        int consecutiveSuccessCountBeforeAttempt = reviewState.ConsecutiveSuccessCount;
        int consecutiveFailureCountBeforeAttempt = reviewState.ConsecutiveFailureCount;
        DateTime? dueAtUtcBeforeAttempt = reviewState.DueAtUtc;
        DateTime dueAtUtcAfterAttempt = PracticeSchedulingPolicy.GetNextDueAtUtc(
            outcome,
            consecutiveSuccessCountBeforeAttempt,
            consecutiveFailureCountBeforeAttempt,
            attemptedAtUtc);

        PracticeAttempt attempt = new(
            Guid.NewGuid(),
            LocalUserId,
            wordEntryPublicId,
            sessionType,
            outcome,
            attemptedAtUtc,
            dueAtUtcBeforeAttempt,
            dueAtUtcAfterAttempt,
            responseMilliseconds);

        reviewState.RecordAttempt(
            sessionType,
            outcome,
            attemptedAtUtc,
            dueAtUtcAfterAttempt);

        if (dbContext.Entry(reviewState).State == EntityState.Detached)
        {
            dbContext.PracticeReviewStates.Add(reviewState);
        }

        dbContext.PracticeAttempts.Add(attempt);

        return new PracticeAnswerSubmissionResult(
            reviewState.WordEntryPublicId,
            outcome,
            attemptedAtUtc,
            dueAtUtcBeforeAttempt,
            dueAtUtcAfterAttempt,
            reviewState.TotalAttemptCount,
            reviewState.ConsecutiveSuccessCount,
            reviewState.ConsecutiveFailureCount);
    }

    private static Task<bool> IsTrackedActiveWordAsync(
        DarwinLinguaDbContext dbContext,
        Guid wordEntryPublicId,
        CancellationToken cancellationToken)
    {
        return (
            from userWordState in dbContext.UserWordStates
            join wordEntry in dbContext.WordEntries
                on userWordState.WordEntryPublicId equals wordEntry.PublicId
            where userWordState.UserId == LocalUserId &&
                userWordState.WordEntryPublicId == wordEntryPublicId &&
                wordEntry.PublicationStatus == PublicationStatus.Active
            select userWordState.WordEntryPublicId)
            .AnyAsync(cancellationToken);
    }
}
