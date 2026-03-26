using DarwinLingua.Localization.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DarwinLingua.SharedKernel.Globalization;

namespace DarwinLingua.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configures persistence details for the <see cref="Language"/> entity.
/// </summary>
internal sealed class LanguageConfiguration : IEntityTypeConfiguration<Language>
{
    /// <summary>
    /// Configures the entity mapping for the <see cref="Language"/> table.
    /// </summary>
    /// <param name="builder">The entity builder used by EF Core.</param>
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("Languages");

        builder.HasKey(language => language.Id);

        builder.Property(language => language.Id)
            .ValueGeneratedNever();

        builder.Property(language => language.Code)
            .HasConversion(
                languageCode => languageCode.Value,
                value => LanguageCode.From(value))
            .HasMaxLength(16)
            .IsRequired();

        builder.Property(language => language.EnglishName)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(language => language.NativeName)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(language => language.IsActive)
            .IsRequired();

        builder.Property(language => language.SupportsUserInterface)
            .IsRequired();

        builder.Property(language => language.SupportsMeanings)
            .IsRequired();

        builder.HasIndex(language => language.Code)
            .IsUnique();
    }
}
