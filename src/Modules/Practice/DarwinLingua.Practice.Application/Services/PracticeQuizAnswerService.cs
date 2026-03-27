using DarwinLingua.Practice.Application.Abstractions;
using DarwinLingua.Practice.Application.Models;
using DarwinLingua.Practice.Domain.Entities;

namespace DarwinLingua.Practice.Application.Services;

/// <summary>
/// Persists quiz answers and updates the learner's review scheduling state.
/// </summary>
internal sealed class PracticeQuizAnswerService : IPracticeQuizAnswerService
{
    private readonly PracticeAnswerSubmissionService _practiceAnswerSubmissionService;

    /// <summary>
    /// Initializes a new instance of the <see cref="PracticeQuizAnswerService"/> class.
    /// </summary>
    public PracticeQuizAnswerService(PracticeAnswerSubmissionService practiceAnswerSubmissionService)
    {
        ArgumentNullException.ThrowIfNull(practiceAnswerSubmissionService);

        _practiceAnswerSubmissionService = practiceAnswerSubmissionService;
    }

    /// <inheritdoc />
    public async Task<PracticeQuizAnswerResultModel> SubmitAsync(
        PracticeQuizAnswerRequestModel request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        PracticeAnswerSubmissionResult result = await _practiceAnswerSubmissionService.SubmitAsync(
            request.WordEntryPublicId,
            PracticeSessionType.Quiz,
            request.Outcome,
            request.ResponseMilliseconds,
            request.AttemptedAtUtc,
            cancellationToken).ConfigureAwait(false);

        return new PracticeQuizAnswerResultModel(
            result.WordEntryPublicId,
            result.Outcome,
            result.AttemptedAtUtc,
            result.DueAtUtcBeforeAttempt,
            result.DueAtUtcAfterAttempt,
            result.TotalAttemptCount,
            result.ConsecutiveSuccessCount,
            result.ConsecutiveFailureCount);
    }
}
