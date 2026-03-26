using DarwinLingua.SharedKernel.Exceptions;
using DarwinLingua.SharedKernel.Globalization;

namespace DarwinLingua.Catalog.Domain.Entities;

/// <summary>
/// Represents the localized display text of a topic in one supported language.
/// </summary>
public sealed class TopicLocalization
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TopicLocalization"/> class for EF Core materialization.
    /// </summary>
    private TopicLocalization()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TopicLocalization"/> class.
    /// </summary>
    internal TopicLocalization(
        Guid id,
        Guid topicId,
        LanguageCode languageCode,
        string displayName,
        DateTime createdAtUtc,
        DateTime updatedAtUtc)
    {
        if (id == Guid.Empty)
        {
            throw new DomainRuleException("Topic localization identifier cannot be empty.");
        }

        if (topicId == Guid.Empty)
        {
            throw new DomainRuleException("Topic localization must belong to a topic.");
        }

        Id = id;
        TopicId = topicId;
        LanguageCode = languageCode;
        DisplayName = NormalizeDisplayName(displayName);
        CreatedAtUtc = NormalizeUtc(createdAtUtc, nameof(createdAtUtc));
        UpdatedAtUtc = NormalizeUtc(updatedAtUtc, nameof(updatedAtUtc));
    }

    /// <summary>
    /// Gets the stable identifier of the localization row.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the identifier of the owning topic.
    /// </summary>
    public Guid TopicId { get; private set; }

    /// <summary>
    /// Gets the language code of the localized display name.
    /// </summary>
    public LanguageCode LanguageCode { get; private set; }

    /// <summary>
    /// Gets the localized display name.
    /// </summary>
    public string DisplayName { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the creation timestamp in UTC.
    /// </summary>
    public DateTime CreatedAtUtc { get; private set; }

    /// <summary>
    /// Gets the last update timestamp in UTC.
    /// </summary>
    public DateTime UpdatedAtUtc { get; private set; }

    /// <summary>
    /// Updates the localized display name.
    /// </summary>
    internal void UpdateDisplayName(string displayName, DateTime updatedAtUtc)
    {
        DisplayName = NormalizeDisplayName(displayName);
        UpdatedAtUtc = NormalizeUtc(updatedAtUtc, nameof(updatedAtUtc));
    }

    private static string NormalizeDisplayName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainRuleException("Topic localization display name cannot be empty.");
        }

        return value.Trim();
    }

    private static DateTime NormalizeUtc(DateTime value, string parameterName)
    {
        if (value == default)
        {
            throw new DomainRuleException($"{parameterName} cannot be empty.");
        }

        return value.Kind == DateTimeKind.Utc ? value : value.ToUniversalTime();
    }
}
