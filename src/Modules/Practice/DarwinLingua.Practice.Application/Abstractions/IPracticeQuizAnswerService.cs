using DarwinLingua.Practice.Application.Models;

namespace DarwinLingua.Practice.Application.Abstractions;

/// <summary>
/// Coordinates quiz-answer submission workflows.
/// </summary>
public interface IPracticeQuizAnswerService
{
    /// <summary>
    /// Persists one quiz answer and updates scheduling state.
    /// </summary>
    Task<PracticeQuizAnswerResultModel> SubmitAsync(
        PracticeQuizAnswerRequestModel request,
        CancellationToken cancellationToken);
}
