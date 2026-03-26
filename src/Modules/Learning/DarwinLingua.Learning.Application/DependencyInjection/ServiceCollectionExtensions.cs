using Microsoft.Extensions.DependencyInjection;

namespace DarwinLingua.Learning.Application.DependencyInjection;

/// <summary>
/// Registers learning application services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the learning application module to the service collection.
    /// </summary>
    /// <param name="services">The service collection being configured.</param>
    /// <returns>The same service collection for chaining.</returns>
    public static IServiceCollection AddLearningApplication(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        return services;
    }
}
