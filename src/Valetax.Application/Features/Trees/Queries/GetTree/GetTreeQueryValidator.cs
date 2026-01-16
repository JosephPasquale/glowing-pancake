using FluentValidation;

namespace Valetax.Application.Features.Trees.Queries.GetTree;

/// <summary>
/// Validator for GetTreeQuery.
/// </summary>
public sealed class GetTreeQueryValidator : AbstractValidator<GetTreeQuery>
{
    public GetTreeQueryValidator()
    {
        RuleFor(x => x.TreeName)
            .NotEmpty()
            .WithMessage("Tree name is required")
            .MaximumLength(100)
            .WithMessage("Tree name cannot exceed 100 characters");
    }
}
