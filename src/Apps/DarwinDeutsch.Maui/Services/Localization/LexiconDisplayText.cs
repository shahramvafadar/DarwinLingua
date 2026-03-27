using DarwinDeutsch.Maui.Resources.Strings;

namespace DarwinDeutsch.Maui.Services.Localization;

/// <summary>
/// Builds localized UI text for lexical metadata shown in MAUI screens.
/// </summary>
internal static class LexiconDisplayText
{
    /// <summary>
    /// Returns a localized metadata line for part of speech and CEFR level.
    /// </summary>
    public static string FormatMetadata(string partOfSpeech, string cefrLevel)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(partOfSpeech);
        ArgumentException.ThrowIfNullOrWhiteSpace(cefrLevel);

        return string.Format(
            AppStrings.LexiconMetadataFormat,
            GetPartOfSpeechDisplayName(partOfSpeech),
            cefrLevel);
    }

    /// <summary>
    /// Returns the localized display name for a part-of-speech token.
    /// </summary>
    public static string GetPartOfSpeechDisplayName(string partOfSpeech)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(partOfSpeech);

        return partOfSpeech switch
        {
            "Noun" => AppStrings.PartOfSpeechNoun,
            "Verb" => AppStrings.PartOfSpeechVerb,
            "Adjective" => AppStrings.PartOfSpeechAdjective,
            "Adverb" => AppStrings.PartOfSpeechAdverb,
            "Pronoun" => AppStrings.PartOfSpeechPronoun,
            "Preposition" => AppStrings.PartOfSpeechPreposition,
            "Conjunction" => AppStrings.PartOfSpeechConjunction,
            "Interjection" => AppStrings.PartOfSpeechInterjection,
            "Numeral" => AppStrings.PartOfSpeechNumeral,
            "Phrase" => AppStrings.PartOfSpeechPhrase,
            "Expression" => AppStrings.PartOfSpeechExpression,
            "Other" => AppStrings.PartOfSpeechOther,
            _ => partOfSpeech,
        };
    }
}
