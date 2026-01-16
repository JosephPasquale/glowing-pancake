using FluentValidation;

namespace Valetax.Application.Features.Trees.Commands.CreateNode;

/// <summary>
/// Validator for CreateNodeCommand.
/// </summary>
public sealed class CreateNodeCommandValidator : AbstractValidator<CreateNodeCommand>
{
    public CreateNodeCommandValidator()
    {
        RuleFor(x => x.TreeName)
            .NotEmpty()
            .WithMessage("Tree name is required")
            .MaximumLength(100)
            .WithMessage("Tree name cannot exceed 100 characters");

        RuleFor(x => x.NodeName)
            .NotEmpty()
            .WithMessage("Node name is required")
            .MaximumLength(100)
            .WithMessage("Node name cannot exceed 100 characters");

        RuleFor(x => x.ParentNodeId)
            .GreaterThan(0)
            .When(x => x.ParentNodeId.HasValue)
            .WithMessage("Parent node ID must be greater than 0");
    }
}
