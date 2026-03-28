namespace DarwinDeutsch.Maui.Services.Browse.Models;

/// <summary>
/// Describes previous/next navigation inside one CEFR browse set.
/// </summary>
public sealed record CefrBrowseNavigationState(
    Guid? PreviousWordPublicId,
    Guid? NextWordPublicId,
    int CurrentIndex,
    int TotalCount);
