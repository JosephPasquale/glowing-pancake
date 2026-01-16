using MediatR;
using Valetax.Application.Common.Interfaces;
using Valetax.Domain.Exceptions;
using Valetax.Domain.Repositories;
using Valetax.Domain.ValueObjects;

namespace Valetax.Application.Features.Trees.Commands.RenameNode;

public sealed class RenameNodeCommandHandler : IRequestHandler<RenameNodeCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public RenameNodeCommandHandler(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
    {
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task Handle(RenameNodeCommand request, CancellationToken cancellationToken)
    {
        var node = await _unitOfWork.Nodes.GetByIdAsync(request.NodeId, cancellationToken);

        if (node is null)
            throw new NodeNotFoundException(request.NodeId);

        var siblingExists = await _unitOfWork.Nodes.ExistsSiblingWithNameAsync(
            node.TreeId,
            node.ParentId,
            request.NewNodeName,
            node.Id,
            cancellationToken);

        if (siblingExists)
            throw new DuplicateNodeNameException(request.NewNodeName);

        var newName = NodeName.Create(request.NewNodeName);
        node.Rename(newName, _dateTimeProvider.UtcNow);

        _unitOfWork.Nodes.Update(node);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
