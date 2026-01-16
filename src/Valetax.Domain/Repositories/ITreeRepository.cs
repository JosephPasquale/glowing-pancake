using Valetax.Domain.Entities;

namespace Valetax.Domain.Repositories;

/// <summary>
/// Repository interface for Tree aggregate.
/// </summary>
public interface ITreeRepository : IRepository<Tree>
{
    Task<Tree?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<Tree?> GetByNameWithNodesAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string name, CancellationToken cancellationToken = default);
}
