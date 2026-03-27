using DarwinLingua.SharedKernel.Exceptions;

namespace DarwinLingua.Catalog.Domain.Entities;

/// <summary>
/// Represents a learner-facing lexical relation attached to a word entry.
/// </summary>
public sealed class WordRelation
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WordRelation"/> class for EF Core materialization.
    /// </summary>
    private WordRelation()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WordRelation"/> class.
    /// </summary>
    internal WordRelation(
        Guid id,
        Guid wordEntryId,
        WordRelationKind kind,
        string lemma,
        string? note,
        int sortOrder,
        DateTime createdAtUtc)
    {
        if (id == Guid.Empty)
        {
            throw new DomainRuleException("Word relation identifier cannot be empty.");
        }

        if (wordEntryId == Guid.Empty)
        {
            throw new DomainRuleException("Word entry identifier cannot be empty for a relation.");
        }

        if (sortOrder <= 0)
        {
            throw new DomainRuleException("Word relation sort order must be greater than zero.");
        }

        Id = id;
        WordEntryId = wordEntryId;
        Kind = kind;
        Lemma = NormalizeRequiredText(lemma, "Word relation lemma cannot be empty.", 128);
        Note = NormalizeOptionalText(note, 256);
        SortOrder = sortOrder;
        CreatedAtUtc = NormalizeUtc(createdAtUtc, nameof(createdAtUtc));
    }

    public Guid Id { get; private set; }

    public Guid WordEntryId { get; private set; }

    public WordRelationKind Kind { get; private set; }

    public string Lemma { get; private set; } = string.Empty;

    public string? Note { get; private set; }

    public int SortOrder { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    private static string NormalizeRequiredText(string value, string errorMessage, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainRuleException(errorMessage);
        }

        string normalized = string.Join(" ", value.Trim().Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));

        if (normalized.Length > maxLength)
        {
            throw new DomainRuleException($"Word relation text cannot be longer than {maxLength} characters.");
        }

        return normalized;
    }

    private static string? NormalizeOptionalText(string? value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        string normalized = string.Join(" ", value.Trim().Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));

        if (normalized.Length > maxLength)
        {
            throw new DomainRuleException($"Word relation note cannot be longer than {maxLength} characters.");
        }

        return normalized;
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
