using Microsoft.EntityFrameworkCore;
using Valetax.Domain.Entities;
using Valetax.Domain.Repositories;

namespace Valetax.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository implementation for ExceptionJournal aggregate.
/// </summary>
public sealed class ExceptionJournalRepository : RepositoryBase<ExceptionJournal>, IExceptionJournalRepository
{
    public ExceptionJournalRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<ExceptionJournal?> GetByEventIdAsync(long eventId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(j => j.EventId == eventId, cancellationToken);
    }

    public async Task<(IReadOnlyList<ExceptionJournal> Items, int TotalCount)> GetRangeAsync(
        int skip,
        int take,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsQueryable();

        if (fromDate.HasValue)
        {
            query = query.Where(j => j.CreatedAt >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(j => j.CreatedAt <= toDate.Value);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(j =>
                j.ExceptionMessage.Contains(search) ||
                j.ExceptionType.Contains(search) ||
                j.RequestPath.Contains(search));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(j => j.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
