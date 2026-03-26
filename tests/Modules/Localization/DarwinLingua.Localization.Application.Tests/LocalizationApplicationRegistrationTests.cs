using DarwinLingua.Localization.Application.Abstractions;
using DarwinLingua.Localization.Application.DependencyInjection;
using DarwinLingua.Localization.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using DarwinLingua.SharedKernel.Globalization;

namespace DarwinLingua.Localization.Application.Tests;

/// <summary>
/// Verifies the localization application registrations and mapping behavior.
/// </summary>
public sealed class LocalizationApplicationRegistrationTests
{
    /// <summary>
    /// Verifies that the registered query service returns mapped language models.
    /// </summary>
    [Fact]
    public async Task AddLocalizationApplication_ShouldResolveLanguageQueryService()
    {
        ServiceCollection services = new();
        services.AddLocalizationApplication();
        services.AddSingleton<ILanguageRepository>(new FakeLanguageRepository(
        [
            new Language(Guid.NewGuid(), LanguageCode.From("en"), "English", "English", true, true, true),
            new Language(Guid.NewGuid(), LanguageCode.From("de"), "German", "Deutsch", true, true, false),
        ]));

        await using var serviceProvider = services.BuildServiceProvider();

        ILanguageQueryService queryService = serviceProvider.GetRequiredService<ILanguageQueryService>();

        IReadOnlyList<Models.SupportedLanguageModel> languages =
            await queryService.GetActiveLanguagesAsync(CancellationToken.None);

        Assert.Collection(
            languages,
            language =>
            {
                Assert.Equal("en", language.Code);
                Assert.True(language.SupportsMeanings);
            },
            language =>
            {
                Assert.Equal("de", language.Code);
                Assert.False(language.SupportsMeanings);
            });
    }

    /// <summary>
    /// Provides a simple fake repository for query-service tests.
    /// </summary>
    /// <param name="languages">The languages returned by the fake repository.</param>
    private sealed class FakeLanguageRepository(IReadOnlyList<Language> languages) : ILanguageRepository
    {
        /// <inheritdoc />
        public Task<IReadOnlyList<Language>> GetActiveAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(languages);
        }
    }
}
