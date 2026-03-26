using DarwinLingua.Infrastructure.Persistence.Configurations;
using DarwinLingua.Localization.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DarwinLingua.Infrastructure.Persistence;

/// <summary>
/// Represents the shared Phase 1 EF Core database context used by the current local-first hosts.
/// </summary>
public sealed class DarwinLinguaDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DarwinLinguaDbContext"/> class.
    /// </summary>
    /// <param name="options">The configured EF Core options.</param>
    public DarwinLinguaDbContext(DbContextOptions<DarwinLinguaDbContext> options)
        : base(options)
    {
        ArgumentNullException.ThrowIfNull(options);
    }

    /// <summary>
    /// Gets the language reference data set.
    /// </summary>
    public DbSet<Language> Languages => Set<Language>();

    /// <summary>
    /// Applies all explicit entity configurations.
    /// </summary>
    /// <param name="modelBuilder">The EF Core model builder.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        modelBuilder.ApplyConfiguration(new LanguageConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
