using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Valetax.Domain.Entities;

namespace Valetax.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for Node entity.
/// </summary>
public sealed class NodeConfiguration : IEntityTypeConfiguration<Node>
{
    public void Configure(EntityTypeBuilder<Node> builder)
    {
        builder.ToTable("Nodes");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(n => n.TreeId)
            .IsRequired();

        builder.Property(n => n.CreatedAt)
            .IsRequired();

        builder.HasOne(n => n.Tree)
            .WithMany()
            .HasForeignKey(n => n.TreeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(n => n.Parent)
            .WithMany(n => n.Children)
            .HasForeignKey(n => n.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Unique constraint: node name must be unique among siblings
        builder.HasIndex(n => new { n.TreeId, n.ParentId, n.Name })
            .IsUnique();

        // Index for tree lookup
        builder.HasIndex(n => n.TreeId);

        // Index for parent lookup
        builder.HasIndex(n => n.ParentId);
    }
}
