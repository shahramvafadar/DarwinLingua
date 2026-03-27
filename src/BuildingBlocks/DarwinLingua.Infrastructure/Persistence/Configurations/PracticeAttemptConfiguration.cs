using DarwinLingua.Practice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DarwinLingua.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configures persistence details for the <see cref="PracticeAttempt"/> entity.
/// </summary>
internal sealed class PracticeAttemptConfiguration : IEntityTypeConfiguration<PracticeAttempt>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<PracticeAttempt> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("PracticeAttempts");

        builder.HasKey(practiceAttempt => practiceAttempt.Id);

        builder.Property(practiceAttempt => practiceAttempt.Id)
            .ValueGeneratedNever();

        builder.Property(practiceAttempt => practiceAttempt.UserId)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(practiceAttempt => practiceAttempt.WordEntryPublicId)
            .IsRequired();

        builder.Property(practiceAttempt => practiceAttempt.SessionType)
            .HasConversion<string>()
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(practiceAttempt => practiceAttempt.Outcome)
            .HasConversion<string>()
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(practiceAttempt => practiceAttempt.AttemptedAtUtc)
            .IsRequired();

        builder.Property(practiceAttempt => practiceAttempt.CreatedAtUtc)
            .IsRequired();

        builder.HasIndex(practiceAttempt => new
            {
                practiceAttempt.UserId,
                practiceAttempt.AttemptedAtUtc,
            })
            .HasDatabaseName("IX_PracticeAttempts_User_AttemptedAtUtc");

        builder.HasIndex(practiceAttempt => new
            {
                practiceAttempt.UserId,
                practiceAttempt.WordEntryPublicId,
                practiceAttempt.AttemptedAtUtc,
            })
            .HasDatabaseName("IX_PracticeAttempts_User_Word_AttemptedAtUtc");
    }
}
