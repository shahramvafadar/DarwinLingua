using Microsoft.Extensions.DependencyInjection;
using DarwinLingua.Localization.Application.Abstractions;
using DarwinLingua.Localization.Application.Services;

namespace DarwinLingua.Localization.Application.DependencyInjection;

/// <summary>
/// Registers localization application services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the localization application module to the service collection.
    /// </summary>
    /// <param name="services">The service collection being configured.</param>
    /// <returns>The same service collection for chaining.</returns>
    public static IServiceCollection AddLocalizationApplication(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<ILanguageQueryService, LanguageQueryService>();

        return services;
    }
}
