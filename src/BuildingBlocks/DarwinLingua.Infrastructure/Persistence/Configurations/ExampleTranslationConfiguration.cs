using DarwinLingua.Catalog.Domain.Entities;
using DarwinLingua.SharedKernel.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DarwinLingua.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configures persistence details for the <see cref="ExampleTranslation"/> entity.
/// </summary>
internal sealed class ExampleTranslationConfiguration : IEntityTypeConfiguration<ExampleTranslation>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<ExampleTranslation> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("ExampleTranslations");

        builder.HasKey(translation => translation.Id);

        builder.Property(translation => translation.Id)
            .ValueGeneratedNever();

        builder.Property(translation => translation.ExampleSentenceId)
            .IsRequired();

        builder.Property(translation => translation.LanguageCode)
            .HasConversion(
                languageCode => languageCode.Value,
                value => LanguageCode.From(value))
            .HasMaxLength(16)
            .IsRequired();

        builder.Property(translation => translation.TranslationText)
            .HasMaxLength(1024)
            .IsRequired();

        builder.Property(translation => translation.CreatedAtUtc)
            .IsRequired();

        builder.Property(translation => translation.UpdatedAtUtc)
            .IsRequired();

        builder.HasIndex(translation => new { translation.ExampleSentenceId, translation.LanguageCode })
            .IsUnique();
    }
}
