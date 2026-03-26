using DarwinLingua.Localization.Application.Abstractions;
using DarwinLingua.Localization.Application.Models;

namespace DarwinLingua.Localization.Application.Services;

/// <summary>
/// Provides read-only localization language queries for presentation consumers.
/// </summary>
internal sealed class LanguageQueryService : ILanguageQueryService
{
    private readonly ILanguageRepository _languageRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="LanguageQueryService"/> class.
    /// </summary>
    /// <param name="languageRepository">The repository used to access language reference data.</param>
    public LanguageQueryService(ILanguageRepository languageRepository)
    {
        ArgumentNullException.ThrowIfNull(languageRepository);

        _languageRepository = languageRepository;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<SupportedLanguageModel>> GetActiveLanguagesAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<Domain.Entities.Language> languages = await _languageRepository
            .GetActiveAsync(cancellationToken)
            .ConfigureAwait(false);

        return languages
            .Select(language => new SupportedLanguageModel(
                language.Code.Value,
                language.EnglishName,
                language.NativeName,
                language.SupportsUserInterface,
                language.SupportsMeanings))
            .ToArray();
    }
}
