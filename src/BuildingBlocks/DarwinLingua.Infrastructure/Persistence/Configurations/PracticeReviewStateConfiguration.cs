using DarwinLingua.Practice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DarwinLingua.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configures persistence details for the <see cref="PracticeReviewState"/> entity.
/// </summary>
internal sealed class PracticeReviewStateConfiguration : IEntityTypeConfiguration<PracticeReviewState>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<PracticeReviewState> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("PracticeReviewStates");

        builder.HasKey(practiceReviewState => practiceReviewState.Id);

        builder.Property(practiceReviewState => practiceReviewState.Id)
            .ValueGeneratedNever();

        builder.Property(practiceReviewState => practiceReviewState.UserId)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(practiceReviewState => practiceReviewState.WordEntryPublicId)
            .IsRequired();

        builder.Property(practiceReviewState => practiceReviewState.LastSessionType)
            .HasConversion<string>()
            .HasMaxLength(32);

        builder.Property(practiceReviewState => practiceReviewState.LastOutcome)
            .HasConversion<string>()
            .HasMaxLength(32);

        builder.Property(practiceReviewState => practiceReviewState.ConsecutiveSuccessCount)
            .IsRequired();

        builder.Property(practiceReviewState => practiceReviewState.ConsecutiveFailureCount)
            .IsRequired();

        builder.Property(practiceReviewState => practiceReviewState.TotalAttemptCount)
            .IsRequired();

        builder.Property(practiceReviewState => practiceReviewState.CreatedAtUtc)
            .IsRequired();

        builder.Property(practiceReviewState => practiceReviewState.UpdatedAtUtc)
            .IsRequired();

        builder.HasIndex(practiceReviewState => new
            {
                practiceReviewState.UserId,
                practiceReviewState.WordEntryPublicId,
            })
            .IsUnique();

        builder.HasIndex(practiceReviewState => new
            {
                practiceReviewState.UserId,
                practiceReviewState.DueAtUtc,
            })
            .HasDatabaseName("IX_PracticeReviewStates_User_DueAtUtc");
    }
}
