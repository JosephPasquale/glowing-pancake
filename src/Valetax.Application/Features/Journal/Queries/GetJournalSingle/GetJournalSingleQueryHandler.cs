using MediatR;
using Valetax.Domain.Repositories;

namespace Valetax.Application.Features.Journal.Queries.GetJournalSingle;

/// <summary>
/// Handler for GetJournalSingleQuery.
/// </summary>
public sealed class GetJournalSingleQueryHandler : IRequestHandler<GetJournalSingleQuery, JournalDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetJournalSingleQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<JournalDto?> Handle(GetJournalSingleQuery request, CancellationToken cancellationToken)
    {
        var journal = await _unitOfWork.ExceptionJournals.GetByIdAsync(request.Id, cancellationToken);

        if (journal is null)
            return null;

        var text = $"Exception: {journal.ExceptionType}\n" +
                   $"Message: {journal.ExceptionMessage}\n" +
                   $"Path: {journal.RequestPath}\n" +
                   $"Query: {journal.QueryParameters}\n" +
                   $"Body: {journal.BodyParameters ?? "(none)"}\n" +
                   $"StackTrace:\n{journal.StackTrace}";

        return new JournalDto(
            journal.Id,
            journal.EventId,
            journal.CreatedAt,
            text);
    }
}
