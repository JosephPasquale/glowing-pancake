using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Valetax.Application.Features.Journal.Queries.GetJournalRange;
using Valetax.Application.Features.Journal.Queries.GetJournalSingle;

namespace Valetax.API.Controllers;

/// <summary>
/// Controller for exception journal operations.
/// </summary>
[Authorize]
public sealed class JournalController : ApiControllerBase
{
    /// <summary>
    /// Gets a paginated range of journal entries.
    /// </summary>
    /// <param name="skip">Number of items to skip.</param>
    /// <param name="take">Maximum number of items to return.</param>
    /// <param name="filter">Optional filter criteria.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Paginated list of journal entries.</returns>
    [HttpPost("api.user.journal.getRange")]
    public async Task<ActionResult<JournalRangeDto>> GetRange(
        [FromQuery] int skip,
        [FromQuery] int take,
        [FromBody] JournalFilterDto? filter,
        CancellationToken cancellationToken)
    {
        var query = new GetJournalRangeQuery(skip, take, filter);
        var result = await Mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets a single journal entry by ID.
    /// </summary>
    /// <param name="id">The journal entry ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The journal entry details.</returns>
    [HttpPost("api.user.journal.getSingle")]
    public async Task<ActionResult<JournalDto>> GetSingle(
        [FromQuery] long id,
        CancellationToken cancellationToken)
    {
        var query = new GetJournalSingleQuery(id);
        var result = await Mediator.Send(query, cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }
}
