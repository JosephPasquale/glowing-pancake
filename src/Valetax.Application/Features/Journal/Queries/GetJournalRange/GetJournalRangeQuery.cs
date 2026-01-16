using MediatR;

namespace Valetax.Application.Features.Journal.Queries.GetJournalRange;

/// <summary>
/// Query to get a paginated range of journal entries.
/// </summary>
public sealed record GetJournalRangeQuery(
    int Skip,
    int Take,
    JournalFilterDto? Filter) : IRequest<JournalRangeDto>;
