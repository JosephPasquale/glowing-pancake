using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Valetax.Application.Features.Trees.Commands.CreateNode;
using Valetax.Application.Features.Trees.Commands.DeleteNode;
using Valetax.Application.Features.Trees.Commands.RenameNode;

namespace Valetax.API.Controllers;

/// <summary>
/// Controller for tree node operations.
/// </summary>
[Authorize]
public sealed class TreeNodeController : ApiControllerBase
{
    /// <summary>
    /// Creates a new node in the tree.
    /// </summary>
    /// <param name="treeName">The name of the tree.</param>
    /// <param name="parentNodeId">The parent node ID (optional for root nodes).</param>
    /// <param name="nodeName">The name of the new node.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created node ID.</returns>
    [HttpPost("api.user.tree.node.create")]
    public async Task<ActionResult<CreateNodeResult>> CreateNode(
        [FromQuery] string treeName,
        [FromQuery] long? parentNodeId,
        [FromQuery] string nodeName,
        CancellationToken cancellationToken)
    {
        var command = new CreateNodeCommand(treeName, parentNodeId, nodeName);
        var result = await Mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a node and all its descendants.
    /// </summary>
    /// <param name="nodeId">The ID of the node to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpPost("api.user.tree.node.delete")]
    public async Task<IActionResult> DeleteNode(
        [FromQuery] long nodeId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteNodeCommand(nodeId);
        await Mediator.Send(command, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Renames an existing node.
    /// </summary>
    /// <param name="nodeId">The ID of the node to rename.</param>
    /// <param name="newNodeName">The new name for the node.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpPost("api.user.tree.node.rename")]
    public async Task<IActionResult> RenameNode(
        [FromQuery] long nodeId,
        [FromQuery] string newNodeName,
        CancellationToken cancellationToken)
    {
        var command = new RenameNodeCommand(nodeId, newNodeName);
        await Mediator.Send(command, cancellationToken);
        return Ok();
    }
}
