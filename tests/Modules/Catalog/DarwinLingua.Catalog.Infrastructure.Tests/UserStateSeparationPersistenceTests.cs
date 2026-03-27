using DarwinLingua.Catalog.Domain.Entities;
using DarwinLingua.Infrastructure.DependencyInjection;
using DarwinLingua.Infrastructure.Persistence;
using DarwinLingua.Infrastructure.Persistence.Abstractions;
using DarwinLingua.Learning.Domain.Entities;
using DarwinLingua.SharedKernel.Content;
using DarwinLingua.SharedKernel.Globalization;
using DarwinLingua.SharedKernel.Lexicon;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DarwinLingua.Catalog.Infrastructure.Tests;

/// <summary>
/// Verifies that local user state remains separate from mutable catalog content rows.
/// </summary>
public sealed class UserStateSeparationPersistenceTests
{
    /// <summary>
    /// Verifies that deleting catalog content does not cascade-delete user favorites or user word state.
    /// </summary>
    [Fact]
    public async Task DeletingWordEntry_ShouldNotDeleteUserStateRows()
    {
        string databasePath = Path.Combine(Path.GetTempPath(), $"darwin-lingua-user-state-separation-{Guid.NewGuid():N}.db");
        await using ServiceProvider serviceProvider = BuildServiceProvider(databasePath);

        IDatabaseInitializer databaseInitializer = serviceProvider.GetRequiredService<IDatabaseInitializer>();
        await databaseInitializer.EnsureDatabaseSchemaAsync(CancellationToken.None);

        IDbContextFactory<DarwinLinguaDbContext> dbContextFactory =
            serviceProvider.GetRequiredService<IDbContextFactory<DarwinLinguaDbContext>>();

        Guid publicId = Guid.NewGuid();

        await using (DarwinLinguaDbContext setupContext = await dbContextFactory.CreateDbContextAsync(CancellationToken.None))
        {
            WordEntry wordEntry = new(
                Guid.NewGuid(),
                publicId,
                "Bahnhof",
                LanguageCode.From("de"),
                CefrLevel.A1,
                PartOfSpeech.Noun,
                PublicationStatus.Active,
                ContentSourceType.Manual,
                DateTime.UtcNow,
                article: "der");

            setupContext.WordEntries.Add(wordEntry);
            setupContext.UserFavoriteWords.Add(new UserFavoriteWord(
                Guid.NewGuid(),
                "local-installation-user",
                publicId,
                DateTime.UtcNow));
            setupContext.UserWordStates.Add(new UserWordState(
                Guid.NewGuid(),
                "local-installation-user",
                publicId,
                DateTime.UtcNow));

            await setupContext.SaveChangesAsync(CancellationToken.None);
        }

        await using (DarwinLinguaDbContext deleteContext = await dbContextFactory.CreateDbContextAsync(CancellationToken.None))
        {
            WordEntry wordEntry = await deleteContext.WordEntries.SingleAsync(CancellationToken.None);
            deleteContext.WordEntries.Remove(wordEntry);
            await deleteContext.SaveChangesAsync(CancellationToken.None);
        }

        await using DarwinLinguaDbContext verificationContext = await dbContextFactory.CreateDbContextAsync(CancellationToken.None);

        Assert.Equal(0, await verificationContext.WordEntries.CountAsync(CancellationToken.None));
        Assert.Equal(1, await verificationContext.UserFavoriteWords.CountAsync(CancellationToken.None));
        Assert.Equal(1, await verificationContext.UserWordStates.CountAsync(CancellationToken.None));
    }

    private static ServiceProvider BuildServiceProvider(string databasePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(databasePath);

        ServiceCollection services = new();
        services.AddDarwinLinguaInfrastructure(options => options.DatabasePath = databasePath);

        return services.BuildServiceProvider();
    }
}
