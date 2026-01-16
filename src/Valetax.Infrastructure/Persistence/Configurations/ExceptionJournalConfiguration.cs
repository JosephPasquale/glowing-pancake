using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Valetax.Domain.Entities;

namespace Valetax.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for ExceptionJournal entity.
/// </summary>
public sealed class ExceptionJournalConfiguration : IEntityTypeConfiguration<ExceptionJournal>
{
    public void Configure(EntityTypeBuilder<ExceptionJournal> builder)
    {
        builder.ToTable("ExceptionJournals");

        builder.HasKey(j => j.Id);

        builder.Property(j => j.EventId)
            .IsRequired();

        builder.HasIndex(j => j.EventId)
            .IsUnique();

        builder.Property(j => j.CreatedAt)
            .IsRequired();

        builder.HasIndex(j => j.CreatedAt);

        builder.Property(j => j.ExceptionType)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(j => j.ExceptionMessage)
            .IsRequired();

        builder.Property(j => j.StackTrace)
            .IsRequired();

        builder.Property(j => j.RequestPath)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(j => j.QueryParameters)
            .IsRequired();

        builder.Property(j => j.BodyParameters);
    }
}
