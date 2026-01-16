using Valetax.Domain.Entities;

namespace Valetax.Domain.Repositories;

/// <summary>
/// Repository interface for Node entity.
/// </summary>
public interface INodeRepository : IRepository<Node>
{
    Task<Node?> GetByIdWithChildrenAsync(long id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Node>> GetByTreeIdAsync(long treeId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Node>> GetChildrenAsync(long parentId, CancellationToken cancellationToken = default);
    Task<bool> HasChildrenAsync(long nodeId, CancellationToken cancellationToken = default);
    Task<bool> ExistsSiblingWithNameAsync(long treeId, long? parentId, string name, long? excludeNodeId = null, CancellationToken cancellationToken = default);
    Task DeleteWithDescendantsAsync(long nodeId, CancellationToken cancellationToken = default);
}
