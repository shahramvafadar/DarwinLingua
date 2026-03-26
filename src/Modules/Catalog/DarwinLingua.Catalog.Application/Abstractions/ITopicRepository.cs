using DarwinLingua.Catalog.Domain.Entities;

namespace DarwinLingua.Catalog.Application.Abstractions;

/// <summary>
/// Defines persistence access for catalog topic aggregates.
/// </summary>
public interface ITopicRepository
{
    /// <summary>
    /// Returns the stored topics with their localizations.
    /// </summary>
    Task<IReadOnlyList<Topic>> GetAllAsync(CancellationToken cancellationToken);
}
