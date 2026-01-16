using Microsoft.EntityFrameworkCore;
using Valetax.Domain.Entities;
using Valetax.Domain.Repositories;

namespace Valetax.Infrastructure.Persistence.Repositories;

public sealed class NodeRepository : RepositoryBase<Node>, INodeRepository
{
    public NodeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Node?> GetByIdWithChildrenAsync(long id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(n => n.Children)
            .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Node>> GetByTreeIdAsync(long treeId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(n => n.TreeId == treeId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Node>> GetChildrenAsync(long parentId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(n => n.ParentId == parentId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasChildrenAsync(long nodeId, CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(n => n.ParentId == nodeId, cancellationToken);
    }

    public async Task<bool> ExistsSiblingWithNameAsync(
        long treeId,
        long? parentId,
        string name,
        long? excludeNodeId = null,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .Where(n => n.TreeId == treeId && n.ParentId == parentId && n.Name == name);

        if (excludeNodeId.HasValue)
        {
            query = query.Where(n => n.Id != excludeNodeId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    public async Task DeleteWithDescendantsAsync(long nodeId, CancellationToken cancellationToken = default)
    {
        var node = await GetByIdAsync(nodeId, cancellationToken);
        if (node is null)
            return;

        var allTreeNodes = await DbSet
            .Where(n => n.TreeId == node.TreeId)
            .ToListAsync(cancellationToken);

        var descendants = GetAllDescendantsInMemory(nodeId, allTreeNodes);

        foreach (var descendant in descendants.OrderByDescending(d => d.Id))
        {
            DbSet.Remove(descendant);
        }

        DbSet.Remove(node);
    }

    private static List<Node> GetAllDescendantsInMemory(long nodeId, List<Node> allNodes)
    {
        var descendants = new List<Node>();
        var queue = new Queue<long>();
        queue.Enqueue(nodeId);

        while (queue.Count > 0)
        {
            var currentId = queue.Dequeue();
            var children = allNodes.Where(n => n.ParentId == currentId).ToList();

            foreach (var child in children)
            {
                descendants.Add(child);
                queue.Enqueue(child.Id);
            }
        }

        return descendants;
    }
}
