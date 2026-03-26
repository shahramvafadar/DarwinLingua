using DarwinLingua.Learning.Application.Abstractions;
using DarwinLingua.Learning.Application.Models;
using DarwinLingua.Learning.Domain.Entities;
using DarwinLingua.SharedKernel.Exceptions;

namespace DarwinLingua.Learning.Application.Services;

/// <summary>
/// Implements the Phase 1 local favorite-word workflows.
/// </summary>
internal sealed class UserFavoriteWordService : IUserFavoriteWordService
{
    private readonly IUserFavoriteWordRepository _userFavoriteWordRepository;
    private readonly IUserFavoriteWordCatalogReader _userFavoriteWordCatalogReader;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserFavoriteWordService"/> class.
    /// </summary>
    public UserFavoriteWordService(
        IUserFavoriteWordRepository userFavoriteWordRepository,
        IUserFavoriteWordCatalogReader userFavoriteWordCatalogReader)
    {
        ArgumentNullException.ThrowIfNull(userFavoriteWordRepository);
        ArgumentNullException.ThrowIfNull(userFavoriteWordCatalogReader);

        _userFavoriteWordRepository = userFavoriteWordRepository;
        _userFavoriteWordCatalogReader = userFavoriteWordCatalogReader;
    }

    /// <inheritdoc />
    public async Task<bool> IsFavoriteAsync(Guid wordEntryPublicId, CancellationToken cancellationToken)
    {
        if (wordEntryPublicId == Guid.Empty)
        {
            throw new ArgumentException("Word public identifier cannot be empty.", nameof(wordEntryPublicId));
        }

        UserFavoriteWord? favoriteWord = await _userFavoriteWordRepository
            .GetByUserIdAndWordPublicIdAsync(LocalInstallationUser.UserId, wordEntryPublicId, cancellationToken)
            .ConfigureAwait(false);

        return favoriteWord is not null;
    }

    /// <inheritdoc />
    public async Task<bool> ToggleFavoriteAsync(Guid wordEntryPublicId, CancellationToken cancellationToken)
    {
        if (wordEntryPublicId == Guid.Empty)
        {
            throw new ArgumentException("Word public identifier cannot be empty.", nameof(wordEntryPublicId));
        }

        UserFavoriteWord? existingFavorite = await _userFavoriteWordRepository
            .GetByUserIdAndWordPublicIdAsync(LocalInstallationUser.UserId, wordEntryPublicId, cancellationToken)
            .ConfigureAwait(false);

        if (existingFavorite is not null)
        {
            await _userFavoriteWordRepository.DeleteAsync(existingFavorite, cancellationToken).ConfigureAwait(false);
            return false;
        }

        if (!await _userFavoriteWordCatalogReader.ExistsAsync(wordEntryPublicId, cancellationToken).ConfigureAwait(false))
        {
            throw new DomainRuleException("Only active lexical entries can be favorited.");
        }

        UserFavoriteWord favoriteWord = new(
            Guid.NewGuid(),
            LocalInstallationUser.UserId,
            wordEntryPublicId,
            DateTime.UtcNow);

        await _userFavoriteWordRepository.AddAsync(favoriteWord, cancellationToken).ConfigureAwait(false);

        return true;
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<FavoriteWordListItemModel>> GetFavoriteWordsAsync(
        string meaningLanguageCode,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(meaningLanguageCode);

        return _userFavoriteWordCatalogReader.GetFavoriteWordsAsync(
            LocalInstallationUser.UserId,
            meaningLanguageCode,
            cancellationToken);
    }
}
