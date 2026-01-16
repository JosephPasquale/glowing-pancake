using MediatR;

namespace Valetax.Application.Features.Trees.Commands.CreateNode;

/// <summary>
/// Command to create a new node in a tree.
/// </summary>
public sealed record CreateNodeCommand(
    string TreeName,
    long? ParentNodeId,
    string NodeName) : IRequest<CreateNodeResult>;

/// <summary>
/// Result of creating a new node.
/// </summary>
public sealed record CreateNodeResult(long NodeId);
