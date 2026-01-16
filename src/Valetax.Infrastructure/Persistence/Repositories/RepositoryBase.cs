using Microsoft.EntityFrameworkCore;
using Valetax.Domain.Common;
using Valetax.Domain.Repositories;

namespace Valetax.Infrastructure.Persistence.Repositories;

/// <summary>
/// Base repository implementation with common CRUD operations.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
public abstract class RepositoryBase<T> : IRepository<T> where T : Entity
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;

    protected ApplicationDbContext Context => _context;
    protected DbSet<T> DbSet => _dbSet;

    protected RepositoryBase(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync([id], cancellationToken);
    }

    public virtual async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public virtual void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public virtual void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }
}
