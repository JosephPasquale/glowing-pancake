using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Valetax.Domain.Entities;

namespace Valetax.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for User entity.
/// </summary>
public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.UniqueCode)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasIndex(u => u.UniqueCode)
            .IsUnique();

        builder.Property(u => u.CreatedAt)
            .IsRequired();
    }
}
