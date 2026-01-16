using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Valetax.Domain.Entities;

namespace Valetax.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for Tree entity.
/// </summary>
public sealed class TreeConfiguration : IEntityTypeConfiguration<Tree>
{
    public void Configure(EntityTypeBuilder<Tree> builder)
    {
        builder.ToTable("Trees");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(t => t.Name)
            .IsUnique();

        builder.Property(t => t.CreatedAt)
            .IsRequired();

        builder.HasMany(t => t.Nodes)
            .WithOne(n => n.Tree)
            .HasForeignKey(n => n.TreeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(t => t.Nodes);
    }
}
