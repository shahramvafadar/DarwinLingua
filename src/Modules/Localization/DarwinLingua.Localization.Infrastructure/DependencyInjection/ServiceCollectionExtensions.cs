using Microsoft.Extensions.DependencyInjection;
using DarwinLingua.Infrastructure.Persistence.Abstractions;
using DarwinLingua.Localization.Application.Abstractions;
using DarwinLingua.Localization.Infrastructure.Repositories;
using DarwinLingua.Localization.Infrastructure.Seed;

namespace DarwinLingua.Localization.Infrastructure.DependencyInjection;

/// <summary>
/// Registers localization infrastructure services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the localization infrastructure module to the service collection.
    /// </summary>
    /// <param name="services">The service collection being configured.</param>
    /// <returns>The same service collection for chaining.</returns>
    public static IServiceCollection AddLocalizationInfrastructure(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<ILanguageRepository, LanguageRepository>();
        services.AddSingleton<IDatabaseSeeder, LocalizationReferenceDataSeeder>();

        return services;
    }
}
