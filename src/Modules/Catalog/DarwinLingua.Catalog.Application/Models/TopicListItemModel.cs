namespace DarwinLingua.Catalog.Application.Models;

/// <summary>
/// Represents one topic item returned to presentation code.
/// </summary>
public sealed record TopicListItemModel(
    Guid Id,
    string Key,
    string DisplayName,
    int SortOrder);
