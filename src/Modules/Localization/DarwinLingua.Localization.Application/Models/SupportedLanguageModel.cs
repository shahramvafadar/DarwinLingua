namespace DarwinLingua.Localization.Application.Models;

/// <summary>
/// Represents a supported language returned to presentation code.
/// </summary>
public sealed record SupportedLanguageModel(
    string Code,
    string EnglishName,
    string NativeName,
    bool SupportsUserInterface,
    bool SupportsMeanings);
