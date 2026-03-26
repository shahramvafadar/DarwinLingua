using DarwinLingua.Learning.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DarwinLingua.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configures persistence details for the <see cref="UserFavoriteWord"/> entity.
/// </summary>
internal sealed class UserFavoriteWordConfiguration : IEntityTypeConfiguration<UserFavoriteWord>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<UserFavoriteWord> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("UserFavoriteWords");

        builder.HasKey(favoriteWord => favoriteWord.Id);

        builder.Property(favoriteWord => favoriteWord.Id)
            .ValueGeneratedNever();

        builder.Property(favoriteWord => favoriteWord.UserId)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(favoriteWord => favoriteWord.WordEntryPublicId)
            .IsRequired();

        builder.Property(favoriteWord => favoriteWord.CreatedAtUtc)
            .IsRequired();

        builder.HasIndex(favoriteWord => new
            {
                favoriteWord.UserId,
                favoriteWord.WordEntryPublicId,
            })
            .IsUnique();
    }
}
