using MediatR;
using Valetax.Domain.Repositories;

namespace Valetax.Application.Features.Journal.Queries.GetJournalRange;

/// <summary>
/// Handler for GetJournalRangeQuery.
/// </summary>
public sealed class GetJournalRangeQueryHandler : IRequestHandler<GetJournalRangeQuery, JournalRangeDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetJournalRangeQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<JournalRangeDto> Handle(GetJournalRangeQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _unitOfWork.ExceptionJournals.GetRangeAsync(
            request.Skip,
            request.Take,
            request.Filter?.From,
            request.Filter?.To,
            request.Filter?.Search,
            cancellationToken);

        var dtos = items
            .Select(j => new JournalInfoDto(j.Id, j.EventId, j.CreatedAt))
            .ToList();

        return new JournalRangeDto(request.Skip, totalCount, dtos);
    }
}
