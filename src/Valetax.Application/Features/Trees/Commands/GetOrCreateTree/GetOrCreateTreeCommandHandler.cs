using MediatR;
using Valetax.Application.Common.Interfaces;
using Valetax.Application.Features.Trees.Queries.GetTree;
using Valetax.Domain.Repositories;
using Valetax.Domain.ValueObjects;
using TreeEntity = Valetax.Domain.Entities.Tree;
using NodeEntity = Valetax.Domain.Entities.Node;

namespace Valetax.Application.Features.Trees.Commands.GetOrCreateTree;

public sealed class GetOrCreateTreeCommandHandler : IRequestHandler<GetOrCreateTreeCommand, TreeNodeDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public GetOrCreateTreeCommandHandler(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
    {
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<TreeNodeDto?> Handle(GetOrCreateTreeCommand request, CancellationToken cancellationToken)
    {
        var tree = await _unitOfWork.Trees.GetByNameWithNodesAsync(request.TreeName, cancellationToken);

        if (tree is null)
        {
            var treeName = TreeName.Create(request.TreeName);
            tree = TreeEntity.Create(treeName, _dateTimeProvider.UtcNow);
            await _unitOfWork.Trees.AddAsync(tree, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return null;
        }

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
