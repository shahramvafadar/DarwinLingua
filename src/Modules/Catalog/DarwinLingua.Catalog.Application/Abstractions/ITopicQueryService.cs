using DarwinLingua.Catalog.Application.Models;

namespace DarwinLingua.Catalog.Application.Abstractions;

/// <summary>
/// Exposes read-only topic queries used by presentation code.
/// </summary>
public interface ITopicQueryService
{
    /// <summary>
    /// Returns the configured topics localized for the requested UI language.
    /// </summary>
    Task<IReadOnlyList<TopicListItemModel>> GetTopicsAsync(string uiLanguageCode, CancellationToken cancellationToken);
}
