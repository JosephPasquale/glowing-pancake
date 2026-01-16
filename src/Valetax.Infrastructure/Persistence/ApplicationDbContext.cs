using Microsoft.EntityFrameworkCore;
using Valetax.Domain.Entities;

namespace Valetax.Infrastructure.Persistence;

/// <summary>
/// Application database context.
/// </summary>
public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tree> Trees => Set<Tree>();
    public DbSet<Node> Nodes => Set<Node>();
    public DbSet<ExceptionJournal> ExceptionJournals => Set<ExceptionJournal>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
