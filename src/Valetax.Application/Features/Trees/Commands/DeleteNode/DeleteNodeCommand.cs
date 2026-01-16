using MediatR;

namespace Valetax.Application.Features.Trees.Commands.DeleteNode;

/// <summary>
/// Command to delete a node and all its descendants.
/// </summary>
public sealed record DeleteNodeCommand(long NodeId) : IRequest;
