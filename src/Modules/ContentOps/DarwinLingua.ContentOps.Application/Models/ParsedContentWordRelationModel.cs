namespace DarwinLingua.ContentOps.Application.Models;

/// <summary>
/// Represents one parsed lexical relation from an import package entry.
/// </summary>
public sealed record ParsedContentWordRelationModel(
    string Kind,
    string Lemma,
    string? Note);
