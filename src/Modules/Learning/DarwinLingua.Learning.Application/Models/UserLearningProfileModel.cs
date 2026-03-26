namespace DarwinLingua.Learning.Application.Models;

/// <summary>
/// Represents the local user learning profile returned to presentation code.
/// </summary>
public sealed record UserLearningProfileModel(
    string UserId,
    string PreferredMeaningLanguage1,
    string? PreferredMeaningLanguage2,
    string UiLanguageCode);
