using DarwinLingua.Practice.Application.Abstractions;
using DarwinLingua.Practice.Application.Models;
using DarwinLingua.SharedKernel.Globalization;

namespace DarwinLingua.Practice.Application.Services;

/// <summary>
/// Implements learner-facing practice overview workflows.
/// </summary>
internal sealed class PracticeOverviewService : IPracticeOverviewService
{
    private readonly IPracticeOverviewReader _practiceOverviewReader;

    /// <summary>
    /// Initializes a new instance of the <see cref="PracticeOverviewService"/> class.
    /// </summary>
    public PracticeOverviewService(IPracticeOverviewReader practiceOverviewReader)
    {
        ArgumentNullException.ThrowIfNull(practiceOverviewReader);

        _practiceOverviewReader = practiceOverviewReader;
    }

    /// <inheritdoc />
    public Task<PracticeOverviewModel> GetOverviewAsync(string meaningLanguageCode, CancellationToken cancellationToken)
    {
        LanguageCode resolvedMeaningLanguageCode = LanguageCode.From(meaningLanguageCode);
        return _practiceOverviewReader.GetOverviewAsync(resolvedMeaningLanguageCode, cancellationToken);
    }
}
