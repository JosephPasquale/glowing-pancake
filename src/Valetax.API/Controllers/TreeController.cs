using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Valetax.Application.Features.Trees.Commands.GetOrCreateTree;
using Valetax.Application.Features.Trees.Queries.GetTree;

namespace Valetax.API.Controllers;

[Authorize]
public sealed class TreeController : ApiControllerBase
{
    [HttpPost("api.user.tree.get")]
    public async Task<ActionResult<TreeNodeDto>> GetTree(
        [FromQuery] string treeName,
        CancellationToken cancellationToken)
    {
        var command = new GetOrCreateTreeCommand(treeName);
        var result = await Mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
