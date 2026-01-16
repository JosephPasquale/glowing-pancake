using MediatR;

namespace Valetax.Application.Features.Trees.Queries.GetTree;

/// <summary>
/// Query to get a tree by name. If the tree doesn't exist, it will be created.
/// </summary>
public sealed record GetTreeQuery(string TreeName) : IRequest<TreeNodeDto?>;
