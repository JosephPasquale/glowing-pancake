using Microsoft.EntityFrameworkCore;
using Valetax.Domain.Entities;
using Valetax.Domain.Repositories;

namespace Valetax.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository implementation for Tree aggregate.
/// </summary>
public sealed class TreeRepository : RepositoryBase<Tree>, ITreeRepository
{
    public TreeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Tree?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(t => t.Name == name, cancellationToken);
    }

    public async Task<Tree?> GetByNameWithNodesAsync(string name, CancellationToken cancellationToken = default)
    {
        var tree = await DbSet
            .FirstOrDefaultAsync(t => t.Name == name, cancellationToken);

        if (tree is null)
            return null;

        var nodes = await Context.Nodes
            .Where(n => n.TreeId == tree.Id)
            .ToListAsync(cancellationToken);

        tree.LoadNodes(nodes);

        return tree;
    }

    public async Task<bool> ExistsAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(t => t.Name == name, cancellationToken);
    }
}
