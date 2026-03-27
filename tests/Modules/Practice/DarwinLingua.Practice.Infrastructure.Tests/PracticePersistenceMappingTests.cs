using DarwinLingua.Infrastructure.DependencyInjection;
using DarwinLingua.Infrastructure.Persistence;
using DarwinLingua.Practice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace DarwinLingua.Practice.Infrastructure.Tests;

/// <summary>
/// Verifies critical EF Core persistence mappings for Practice entities.
/// </summary>
public sealed class PracticePersistenceMappingTests
{
    /// <summary>
    /// Verifies the unique and scheduling indexes needed by practice review state.
    /// </summary>
    [Fact]
    public async Task Model_ShouldConfigurePracticeReviewStateIndexes()
    {
        string databasePath = Path.Combine(Path.GetTempPath(), $"darwin-lingua-practice-mapping-state-{Guid.NewGuid():N}.db");
        ServiceProvider? serviceProvider = null;

        try
        {
            serviceProvider = BuildServiceProvider(databasePath);
            IDbContextFactory<DarwinLinguaDbContext> dbContextFactory =
                serviceProvider.GetRequiredService<IDbContextFactory<DarwinLinguaDbContext>>();

            await using DarwinLinguaDbContext dbContext = await dbContextFactory.CreateDbContextAsync(CancellationToken.None);

            IEntityType reviewStateEntity = dbContext.Model.FindEntityType(typeof(PracticeReviewState))
                ?? throw new Xunit.Sdk.XunitException("PracticeReviewState mapping is missing from the model.");

            Assert.Contains(
                reviewStateEntity.GetIndexes(),
                index =>
                    index.IsUnique &&
                    index.Properties.Select(property => property.Name).SequenceEqual(
                        [nameof(PracticeReviewState.UserId), nameof(PracticeReviewState.WordEntryPublicId)]));

            Assert.Contains(
                reviewStateEntity.GetIndexes(),
                index =>
                    index.GetDatabaseName() == "IX_PracticeReviewStates_User_DueAtUtc" &&
                    index.Properties.Select(property => property.Name).SequenceEqual(
                        [nameof(PracticeReviewState.UserId), nameof(PracticeReviewState.DueAtUtc)]));
        }
        finally
        {
            if (serviceProvider is not null)
            {
                await serviceProvider.DisposeAsync();
            }

            TryDeleteFile(databasePath);
        }
    }

    /// <summary>
    /// Verifies the historical-attempt indexes needed by practice activity flows.
    /// </summary>
    [Fact]
    public async Task Model_ShouldConfigurePracticeAttemptIndexes()
    {
        string databasePath = Path.Combine(Path.GetTempPath(), $"darwin-lingua-practice-mapping-attempt-{Guid.NewGuid():N}.db");
        ServiceProvider? serviceProvider = null;

        try
        {
            serviceProvider = BuildServiceProvider(databasePath);
            IDbContextFactory<DarwinLinguaDbContext> dbContextFactory =
                serviceProvider.GetRequiredService<IDbContextFactory<DarwinLinguaDbContext>>();

            await using DarwinLinguaDbContext dbContext = await dbContextFactory.CreateDbContextAsync(CancellationToken.None);

            IEntityType attemptEntity = dbContext.Model.FindEntityType(typeof(PracticeAttempt))
                ?? throw new Xunit.Sdk.XunitException("PracticeAttempt mapping is missing from the model.");

            Assert.Contains(
                attemptEntity.GetIndexes(),
                index =>
                    index.GetDatabaseName() == "IX_PracticeAttempts_User_AttemptedAtUtc" &&
                    index.Properties.Select(property => property.Name).SequenceEqual(
                        [nameof(PracticeAttempt.UserId), nameof(PracticeAttempt.AttemptedAtUtc)]));

            Assert.Contains(
                attemptEntity.GetIndexes(),
                index =>
                    index.GetDatabaseName() == "IX_PracticeAttempts_User_Word_AttemptedAtUtc" &&
                    index.Properties.Select(property => property.Name).SequenceEqual(
                        [nameof(PracticeAttempt.UserId), nameof(PracticeAttempt.WordEntryPublicId), nameof(PracticeAttempt.AttemptedAtUtc)]));
        }
        finally
        {
            if (serviceProvider is not null)
            {
                await serviceProvider.DisposeAsync();
            }

            TryDeleteFile(databasePath);
        }
    }

    private static ServiceProvider BuildServiceProvider(string databasePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(databasePath);

        ServiceCollection services = new();
        services.AddDarwinLinguaInfrastructure(options => options.DatabasePath = databasePath);

        return services.BuildServiceProvider();
    }

    private static void TryDeleteFile(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }
    }
}
