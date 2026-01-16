using FluentValidation;

namespace Valetax.Application.Features.Trees.Commands.GetOrCreateTree;

public sealed class GetOrCreateTreeCommandValidator : AbstractValidator<GetOrCreateTreeCommand>
{
    public GetOrCreateTreeCommandValidator()
    {
        RuleFor(x => x.TreeName)
            .NotEmpty().WithMessage("Tree name is required")
            .MaximumLength(100).WithMessage("Tree name cannot exceed 100 characters");
    }
}
