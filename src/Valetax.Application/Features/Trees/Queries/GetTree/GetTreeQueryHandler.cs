using MediatR;
using Valetax.Domain.Repositories;
using TreeEntity = Valetax.Domain.Entities.Tree;
using NodeEntity = Valetax.Domain.Entities.Node;

namespace Valetax.Application.Features.Trees.Queries.GetTree;

public sealed class GetTreeQueryHandler : IRequestHandler<GetTreeQuery, TreeNodeDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTreeQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TreeNodeDto?> Handle(GetTreeQuery request, CancellationToken cancellationToken)
    {
        var tree = await _unitOfWork.Trees.GetByNameWithNodesAsync(request.TreeName, cancellationToken);

        if (tree is null)
            return null;

        return BuildTreeDto(tree);
    }

    private static TreeNodeDto? BuildTreeDto(TreeEntity tree)
    {
        var nodes = tree.Nodes.ToList();

        if (nodes.Count == 0)
            return null;

        var rootNodes = nodes.Where(n => n.ParentId is null).ToList();

        if (rootNodes.Count == 0)
            return null;

        if (rootNodes.Count == 1)
            return BuildNodeDto(rootNodes[0], nodes);

        var children = rootNodes.Select(n => BuildNodeDto(n, nodes)).ToList();
        return new TreeNodeDto(0, tree.Name, children);
    }

    private static TreeNodeDto BuildNodeDto(NodeEntity node, List<NodeEntity> allNodes)
    {
        var children = allNodes
            .Where(n => n.ParentId == node.Id)
            .Select(n => BuildNodeDto(n, allNodes))
            .ToList();

        return new TreeNodeDto(node.Id, node.Name, children);
    }
}
