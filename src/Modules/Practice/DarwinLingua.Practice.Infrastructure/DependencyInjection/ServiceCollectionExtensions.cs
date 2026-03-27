using DarwinLingua.Practice.Application.Abstractions;
using DarwinLingua.Practice.Infrastructure.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace DarwinLingua.Practice.Infrastructure.DependencyInjection;

/// <summary>
/// Registers practice infrastructure services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the practice infrastructure module to the service collection.
    /// </summary>
    public static IServiceCollection AddPracticeInfrastructure(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<IPracticeOverviewReader, PracticeOverviewReader>();

        return services;
    }
}
