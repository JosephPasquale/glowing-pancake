using FluentValidation;

namespace Valetax.Application.Features.Trees.Commands.RenameNode;

/// <summary>
/// Validator for RenameNodeCommand.
/// </summary>
public sealed class RenameNodeCommandValidator : AbstractValidator<RenameNodeCommand>
{
    public RenameNodeCommandValidator()
    {
        RuleFor(x => x.NodeId)
            .GreaterThan(0)
            .WithMessage("Node ID must be greater than 0");

        RuleFor(x => x.NewNodeName)
            .NotEmpty()
            .WithMessage("New node name is required")
            .MaximumLength(100)
            .WithMessage("New node name cannot exceed 100 characters");
    }
}
