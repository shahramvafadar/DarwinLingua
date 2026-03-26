using DarwinLingua.SharedKernel.Globalization;

namespace DarwinLingua.Learning.Application.Abstractions;

/// <summary>
/// Validates whether language codes are supported for user preference scenarios.
/// </summary>
public interface ILearningPreferenceLanguageValidator
{
    /// <summary>
    /// Determines whether the specified language can be used for UI localization.
    /// </summary>
    Task<bool> SupportsUserInterfaceAsync(LanguageCode languageCode, CancellationToken cancellationToken);

    /// <summary>
    /// Determines whether the specified language can be used for meaning translations.
    /// </summary>
    Task<bool> SupportsMeaningsAsync(LanguageCode languageCode, CancellationToken cancellationToken);

    /// <summary>
    /// Returns the supported meaning languages available for the current installation.
    /// </summary>
    Task<IReadOnlyList<LanguageCode>> GetSupportedMeaningLanguagesAsync(CancellationToken cancellationToken);
}
