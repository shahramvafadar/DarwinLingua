using DarwinLingua.Learning.Application.Abstractions;
using DarwinLingua.Learning.Application.DependencyInjection;
using DarwinLingua.Learning.Application.Models;
using DarwinLingua.Learning.Domain.Entities;
using DarwinLingua.SharedKernel.Exceptions;
using DarwinLingua.SharedKernel.Globalization;
using Microsoft.Extensions.DependencyInjection;

namespace DarwinLingua.Learning.Application.Tests;

/// <summary>
/// Verifies the learning-profile application workflows.
/// </summary>
public sealed class UserLearningProfileServiceTests
{
    /// <summary>
    /// Verifies that duplicate meaning languages are rejected by the underlying domain aggregate rules.
    /// </summary>
    [Fact]
    public void UserLearningProfile_ShouldRejectDuplicateMeaningLanguages()
    {
        Assert.Throws<DomainRuleException>(() => new UserLearningProfile(
            Guid.NewGuid(),
            "local-installation-user",
            LanguageCode.From("en"),
            LanguageCode.From("en"),
            LanguageCode.From("de"),
            DateTime.UtcNow));
    }

    /// <summary>
    /// Verifies that a missing local profile is created with a supported default meaning language.
    /// </summary>
    [Fact]
    public async Task EnsureLocalProfileExistsAsync_ShouldCreateDefaultProfile()
    {
        InMemoryUserLearningProfileRepository repository = new();
        FakeLearningPreferenceLanguageValidator validator = new(
            supportedUiLanguages: ["en", "de"],
            supportedMeaningLanguages: ["en"]);
        ServiceCollection services = new();
        services.AddLearningApplication();
        services.AddSingleton<IUserLearningProfileRepository>(repository);
        services.AddSingleton<ILearningPreferenceLanguageValidator>(validator);

        await using ServiceProvider serviceProvider = services.BuildServiceProvider();
        IUserLearningProfileService service = serviceProvider.GetRequiredService<IUserLearningProfileService>();

        UserLearningProfileModel profile = await service
            .EnsureLocalProfileExistsAsync("de", CancellationToken.None);

        Assert.Equal("local-installation-user", profile.UserId);
        Assert.Equal("de", profile.UiLanguageCode);
        Assert.Equal("en", profile.PreferredMeaningLanguage1);
        Assert.Null(profile.PreferredMeaningLanguage2);
    }

    /// <summary>
    /// Verifies that unsupported meaning languages are rejected before persistence.
    /// </summary>
    [Fact]
    public async Task UpdateMeaningLanguagePreferencesAsync_ShouldRejectUnsupportedLanguage()
    {
        InMemoryUserLearningProfileRepository repository = new();
        FakeLearningPreferenceLanguageValidator validator = new(
            supportedUiLanguages: ["en", "de"],
            supportedMeaningLanguages: ["en"]);
        ServiceCollection services = new();
        services.AddLearningApplication();
        services.AddSingleton<IUserLearningProfileRepository>(repository);
        services.AddSingleton<ILearningPreferenceLanguageValidator>(validator);

        await using ServiceProvider serviceProvider = services.BuildServiceProvider();
        IUserLearningProfileService service = serviceProvider.GetRequiredService<IUserLearningProfileService>();

        await service.EnsureLocalProfileExistsAsync("en", CancellationToken.None);

        await Assert.ThrowsAsync<DomainRuleException>(() => service.UpdateMeaningLanguagePreferencesAsync(
            "de",
            preferredMeaningLanguage2: null,
            CancellationToken.None));
    }

    /// <summary>
    /// Stores an in-memory profile for application tests.
    /// </summary>
    private sealed class InMemoryUserLearningProfileRepository : IUserLearningProfileRepository
    {
        private UserLearningProfile? _profile;

        /// <inheritdoc />
        public Task<UserLearningProfile?> GetByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            return Task.FromResult(_profile is not null && _profile.UserId == userId ? _profile : null);
        }

        /// <inheritdoc />
        public Task AddAsync(UserLearningProfile profile, CancellationToken cancellationToken)
        {
            _profile = profile;
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task UpdateAsync(UserLearningProfile profile, CancellationToken cancellationToken)
        {
            _profile = profile;
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// Supplies predictable language-validation rules for application tests.
    /// </summary>
    private sealed class FakeLearningPreferenceLanguageValidator : ILearningPreferenceLanguageValidator
    {
        private readonly HashSet<LanguageCode> _supportedUiLanguages;
        private readonly IReadOnlyList<LanguageCode> _supportedMeaningLanguages;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeLearningPreferenceLanguageValidator"/> class.
        /// </summary>
        public FakeLearningPreferenceLanguageValidator(
            IReadOnlyList<string> supportedUiLanguages,
            IReadOnlyList<string> supportedMeaningLanguages)
        {
            _supportedUiLanguages = supportedUiLanguages.Select(LanguageCode.From).ToHashSet();
            _supportedMeaningLanguages = supportedMeaningLanguages.Select(LanguageCode.From).ToArray();
        }

        /// <inheritdoc />
        public Task<bool> SupportsUserInterfaceAsync(LanguageCode languageCode, CancellationToken cancellationToken)
        {
            return Task.FromResult(_supportedUiLanguages.Contains(languageCode));
        }

        /// <inheritdoc />
        public Task<bool> SupportsMeaningsAsync(LanguageCode languageCode, CancellationToken cancellationToken)
        {
            return Task.FromResult(_supportedMeaningLanguages.Contains(languageCode));
        }

        /// <inheritdoc />
        public Task<IReadOnlyList<LanguageCode>> GetSupportedMeaningLanguagesAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_supportedMeaningLanguages);
        }
    }
}
