using DarwinLingua.Catalog.Application.Abstractions;
using DarwinLingua.Catalog.Application.Models;
using DarwinLingua.Catalog.Domain.Entities;
using DarwinLingua.SharedKernel.Globalization;

namespace DarwinLingua.Catalog.Application.Services;

/// <summary>
/// Provides localized topic lists for presentation consumers.
/// </summary>
internal sealed class TopicQueryService : ITopicQueryService
{
    private static readonly LanguageCode EnglishLanguageCode = LanguageCode.From("en");
    private readonly ITopicRepository _topicRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="TopicQueryService"/> class.
    /// </summary>
    public TopicQueryService(ITopicRepository topicRepository)
    {
        ArgumentNullException.ThrowIfNull(topicRepository);

        _topicRepository = topicRepository;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<TopicListItemModel>> GetTopicsAsync(string uiLanguageCode, CancellationToken cancellationToken)
    {
        LanguageCode requestedLanguageCode = LanguageCode.From(uiLanguageCode);
        IReadOnlyList<Topic> topics = await _topicRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);

        return topics
            .Select(topic => new TopicListItemModel(
                topic.Id,
                topic.Key,
                ResolveDisplayName(topic, requestedLanguageCode),
                topic.SortOrder))
            .OrderBy(topic => topic.SortOrder)
            .ThenBy(topic => topic.DisplayName, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    private static string ResolveDisplayName(Topic topic, LanguageCode requestedLanguageCode)
    {
        ArgumentNullException.ThrowIfNull(topic);

        TopicLocalization? localizedValue =
            topic.FindLocalization(requestedLanguageCode)
            ?? topic.FindLocalization(EnglishLanguageCode)
            ?? topic.Localizations.OrderBy(localization => localization.DisplayName, StringComparer.OrdinalIgnoreCase).FirstOrDefault();

        return localizedValue?.DisplayName ?? topic.Key;
    }
}
