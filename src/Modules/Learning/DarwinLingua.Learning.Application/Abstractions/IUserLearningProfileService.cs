using DarwinLingua.Learning.Application.Models;

namespace DarwinLingua.Learning.Application.Abstractions;

/// <summary>
/// Coordinates the Phase 1 local user learning profile workflows.
/// </summary>
public interface IUserLearningProfileService
{
    /// <summary>
    /// Ensures that the local installation has a usable learning profile.
    /// </summary>
    Task<UserLearningProfileModel> EnsureLocalProfileExistsAsync(
        string requestedUiLanguageCode,
        CancellationToken cancellationToken);

    /// <summary>
    /// Loads the current local learning profile.
    /// </summary>
    Task<UserLearningProfileModel> GetCurrentProfileAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Updates the stored UI language preference.
    /// </summary>
    Task<UserLearningProfileModel> UpdateUiLanguagePreferenceAsync(
        string uiLanguageCode,
        CancellationToken cancellationToken);

    /// <summary>
    /// Updates the stored meaning-language preferences.
    /// </summary>
    Task<UserLearningProfileModel> UpdateMeaningLanguagePreferencesAsync(
        string preferredMeaningLanguage1,
        string? preferredMeaningLanguage2,
        CancellationToken cancellationToken);
}
