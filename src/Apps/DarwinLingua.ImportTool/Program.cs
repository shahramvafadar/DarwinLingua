using DarwinLingua.Catalog.Application.DependencyInjection;
using DarwinLingua.Catalog.Infrastructure.DependencyInjection;
using DarwinLingua.ContentOps.Application.DependencyInjection;
using DarwinLingua.ContentOps.Infrastructure.DependencyInjection;
using DarwinLingua.Infrastructure.DependencyInjection;
using DarwinLingua.Infrastructure.Persistence.Abstractions;
using DarwinLingua.Learning.Application.DependencyInjection;
using DarwinLingua.Learning.Infrastructure.DependencyInjection;
using DarwinLingua.Localization.Application.DependencyInjection;
using DarwinLingua.Localization.Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DarwinLingua.ImportTool;

/// <summary>
/// Bootstraps the Darwin Lingua import tool host.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Creates the import host and validates the current dependency wiring.
    /// </summary>
    /// <param name="args">The command-line arguments passed to the process.</param>
    /// <returns>A task that completes when the host bootstrap finishes.</returns>
    private static async Task Main(string[] args)
    {
        ArgumentNullException.ThrowIfNull(args);

        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        ConfigureLogging(builder);
        RegisterModules(builder.Services);

        using IHost host = builder.Build();

        IDatabaseInitializer databaseInitializer = host.Services.GetRequiredService<IDatabaseInitializer>();
        await databaseInitializer.InitializeAsync(CancellationToken.None).ConfigureAwait(false);

        ILogger logger = host.Services
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("DarwinLingua.ImportTool");
        logger.LogInformation("Darwin Lingua import tool bootstrap is ready and the local database is initialized.");

        await host.StopAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Configures logging for the import tool host.
    /// </summary>
    /// <param name="builder">The host builder being configured.</param>
    private static void ConfigureLogging(HostApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Logging.ClearProviders();
        builder.Logging.AddSimpleConsole(options =>
        {
            options.SingleLine = true;
            options.TimestampFormat = "HH:mm:ss ";
        });
    }

    /// <summary>
    /// Registers the current application and infrastructure modules used by the import tool.
    /// </summary>
    /// <param name="services">The service collection being configured.</param>
    private static void RegisterModules(IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        string databaseDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "DarwinLingua");
        string databasePath = Path.Combine(databaseDirectory, "darwin-lingua.db");

        services
            .AddDarwinLinguaInfrastructure(options => options.DatabasePath = databasePath)
            .AddCatalogApplication()
            .AddCatalogInfrastructure()
            .AddContentOpsApplication()
            .AddContentOpsInfrastructure()
            .AddLearningApplication()
            .AddLearningInfrastructure()
            .AddLocalizationApplication()
            .AddLocalizationInfrastructure();
    }
}
