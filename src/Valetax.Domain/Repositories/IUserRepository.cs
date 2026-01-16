using Valetax.Domain.Entities;

namespace Valetax.Domain.Repositories;

/// <summary>
/// Repository interface for User aggregate.
/// </summary>
public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByUniqueCodeAsync(string uniqueCode, CancellationToken cancellationToken = default);
}
