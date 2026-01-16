namespace Valetax.Application.Features.Trees.Queries.GetTree;

/// <summary>
/// DTO for a tree node with its children.
/// </summary>
public sealed record TreeNodeDto(
    long Id,
    string Name,
    IReadOnlyList<TreeNodeDto> Children);
