using DarwinLingua.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DarwinLingua.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configures persistence details for learner-facing lexical relations.
/// </summary>
internal sealed class WordRelationConfiguration : IEntityTypeConfiguration<WordRelation>
{
    public void Configure(EntityTypeBuilder<WordRelation> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("WordRelations");

        builder.HasKey(relation => relation.Id);

        builder.Property(relation => relation.Id)
            .ValueGeneratedNever();

        builder.Property(relation => relation.WordEntryId)
            .IsRequired();

        builder.Property(relation => relation.Kind)
            .HasConversion<string>()
            .HasMaxLength(16)
            .IsRequired();

        builder.Property(relation => relation.Lemma)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(relation => relation.Note)
            .HasMaxLength(256);

        builder.Property(relation => relation.SortOrder)
            .IsRequired();

        builder.Property(relation => relation.CreatedAtUtc)
            .IsRequired();

        builder.HasIndex(relation => new { relation.WordEntryId, relation.Kind, relation.Lemma })
            .IsUnique();

        builder.HasIndex(relation => new { relation.WordEntryId, relation.Kind, relation.SortOrder })
            .IsUnique();
    }
}
