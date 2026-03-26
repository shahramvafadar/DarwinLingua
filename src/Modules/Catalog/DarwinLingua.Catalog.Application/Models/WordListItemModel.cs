namespace DarwinLingua.Catalog.Application.Models;

/// <summary>
/// Represents a browse-friendly lexical entry summary.
/// </summary>
public sealed record WordListItemModel(
    Guid PublicId,
    string Lemma,
    string? Article,
    string? PluralForm,
    string PartOfSpeech,
    string CefrLevel,
    string? PrimaryMeaning);
