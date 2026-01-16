using MediatR;

namespace Valetax.Application.Features.Trees.Commands.RenameNode;

/// <summary>
/// Command to rename an existing node.
/// </summary>
public sealed record RenameNodeCommand(long NodeId, string NewNodeName) : IRequest;
