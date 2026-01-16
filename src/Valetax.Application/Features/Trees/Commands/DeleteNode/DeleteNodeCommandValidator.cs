using FluentValidation;

namespace Valetax.Application.Features.Trees.Commands.DeleteNode;

/// <summary>
/// Validator for DeleteNodeCommand.
/// </summary>
public sealed class DeleteNodeCommandValidator : AbstractValidator<DeleteNodeCommand>
{
    public DeleteNodeCommandValidator()
    {
        RuleFor(x => x.NodeId)
            .GreaterThan(0)
            .WithMessage("Node ID must be greater than 0");
    }
}
