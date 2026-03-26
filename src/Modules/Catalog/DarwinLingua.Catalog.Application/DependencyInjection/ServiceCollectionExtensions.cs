using Microsoft.Extensions.DependencyInjection;
using DarwinLingua.Catalog.Application.Abstractions;
using DarwinLingua.Catalog.Application.Services;

namespace DarwinLingua.Catalog.Application.DependencyInjection;

/// <summary>
/// Registers catalog application services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the catalog application module to the service collection.
    /// </summary>
    /// <param name="services">The service collection being configured.</param>
    /// <returns>The same service collection for chaining.</returns>
    public static IServiceCollection AddCatalogApplication(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<ITopicQueryService, TopicQueryService>();
        services.AddScoped<IWordDetailQueryService, WordDetailQueryService>();
        services.AddScoped<IWordQueryService, WordQueryService>();

        return services;
    }
}
