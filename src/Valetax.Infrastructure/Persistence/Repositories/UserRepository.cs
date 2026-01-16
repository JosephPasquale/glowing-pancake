using Microsoft.EntityFrameworkCore;
using Valetax.Domain.Entities;
using Valetax.Domain.Repositories;

namespace Valetax.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository implementation for User aggregate.
/// </summary>
public sealed class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByUniqueCodeAsync(string uniqueCode, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(u => u.UniqueCode == uniqueCode, cancellationToken);
    }
}
