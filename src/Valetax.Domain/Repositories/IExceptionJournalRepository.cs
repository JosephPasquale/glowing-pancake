using Valetax.Domain.Entities;

namespace Valetax.Domain.Repositories;

/// <summary>
/// Repository interface for ExceptionJournal aggregate.
/// </summary>
public interface IExceptionJournalRepository : IRepository<ExceptionJournal>
{
    Task<ExceptionJournal?> GetByEventIdAsync(long eventId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<ExceptionJournal> Items, int TotalCount)> GetRangeAsync(
        int skip,
        int take,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        string? search = null,
        CancellationToken cancellationToken = default);
}
