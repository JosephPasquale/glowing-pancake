using MediatR;
using Valetax.Application.Common.Interfaces;
using Valetax.Domain.Exceptions;
using Valetax.Domain.Repositories;
using Valetax.Domain.ValueObjects;
using TreeEntity = Valetax.Domain.Entities.Tree;
using NodeEntity = Valetax.Domain.Entities.Node;

namespace Valetax.Application.Features.Trees.Commands.CreateNode;

public sealed class CreateNodeCommandHandler : IRequestHandler<CreateNodeCommand, CreateNodeResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateNodeCommandHandler(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
    {
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<CreateNodeResult> Handle(CreateNodeCommand request, CancellationToken cancellationToken)
    {
        var now = _dateTimeProvider.UtcNow;

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var tree = await _unitOfWork.Trees.GetByNameWithNodesAsync(request.TreeName, cancellationToken);

            if (tree is null)
            {
                var treeName = TreeName.Create(request.TreeName);
                tree = TreeEntity.Create(treeName, now);
                await _unitOfWork.Trees.AddAsync(tree, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            if (request.ParentNodeId.HasValue)
            {
                var parentNode = await _unitOfWork.Nodes.GetByIdAsync(request.ParentNodeId.Value, cancellationToken);

                if (parentNode is null)
                    throw new NodeNotFoundException(request.ParentNodeId.Value);

                if (parentNode.TreeId != tree.Id)
                    throw new InvalidNodeOperationException("Parent node does not belong to the specified tree");
            }

            var siblingExists = await _unitOfWork.Nodes.ExistsSiblingWithNameAsync(
                tree.Id,
                request.ParentNodeId,
                request.NodeName,
                cancellationToken: cancellationToken);

            if (siblingExists)
                throw new DuplicateNodeNameException(request.NodeName);

            var nodeName = NodeName.Create(request.NodeName);
            var node = NodeEntity.Create(nodeName, tree.Id, request.ParentNodeId, now);

            await _unitOfWork.Nodes.AddAsync(node, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return new CreateNodeResult(node.Id);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
