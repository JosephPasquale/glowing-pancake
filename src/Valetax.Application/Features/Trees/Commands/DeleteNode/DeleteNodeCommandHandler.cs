using MediatR;
using Valetax.Domain.Exceptions;
using Valetax.Domain.Repositories;

namespace Valetax.Application.Features.Trees.Commands.DeleteNode;

/// <summary>
/// Handler for DeleteNodeCommand.
/// </summary>
public sealed class DeleteNodeCommandHandler : IRequestHandler<DeleteNodeCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteNodeCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteNodeCommand request, CancellationToken cancellationToken)
    {
        var node = await _unitOfWork.Nodes.GetByIdAsync(request.NodeId, cancellationToken);

        if (node is null)
            throw new NodeNotFoundException(request.NodeId);

        await _unitOfWork.Nodes.DeleteWithDescendantsAsync(request.NodeId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
