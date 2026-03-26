namespace DarwinLingua.Learning.Application.Models;

/// <summary>
/// Represents a browse-friendly favorite-word summary.
/// </summary>
public sealed record FavoriteWordListItemModel(
    Guid PublicId,
    string Lemma,
    string? Article,
    string PartOfSpeech,
    string CefrLevel,
    string? PrimaryMeaning);
