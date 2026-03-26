using Microsoft.Extensions.DependencyInjection;

namespace DarwinLingua.Catalog.Infrastructure.DependencyInjection;

/// <summary>
/// Registers catalog infrastructure services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the catalog infrastructure module to the service collection.
    /// </summary>
    /// <param name="services">The service collection being configured.</param>
    /// <returns>The same service collection for chaining.</returns>
    public static IServiceCollection AddCatalogInfrastructure(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        return services;
    }
}
