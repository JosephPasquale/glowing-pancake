using MediatR;

namespace Valetax.Application.Features.Journal.Queries.GetJournalSingle;

/// <summary>
/// Query to get a single journal entry by ID.
/// </summary>
public sealed record GetJournalSingleQuery(long Id) : IRequest<JournalDto?>;
