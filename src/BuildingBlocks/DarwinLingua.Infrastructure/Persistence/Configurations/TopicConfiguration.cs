using DarwinLingua.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DarwinLingua.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configures persistence details for the <see cref="Topic"/> aggregate.
/// </summary>
internal sealed class TopicConfiguration : IEntityTypeConfiguration<Topic>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("Topics");

        builder.HasKey(topic => topic.Id);

        builder.Property(topic => topic.Id)
            .ValueGeneratedNever();

        builder.Property(topic => topic.Key)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(topic => topic.SortOrder)
            .IsRequired();

        builder.Property(topic => topic.IsSystem)
            .IsRequired();

        builder.Property(topic => topic.CreatedAtUtc)
            .IsRequired();

        builder.Property(topic => topic.UpdatedAtUtc)
            .IsRequired();

        builder.HasIndex(topic => topic.Key)
            .IsUnique();

        builder.HasMany(topic => topic.Localizations)
            .WithOne()
            .HasForeignKey(localization => localization.TopicId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(topic => topic.Localizations)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
