using FluentValidation;

namespace Valetax.Application.Features.Auth.Commands.RememberMe;

/// <summary>
/// Validator for RememberMeCommand.
/// </summary>
public sealed class RememberMeCommandValidator : AbstractValidator<RememberMeCommand>
{
    public RememberMeCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Code is required")
            .MaximumLength(256)
            .WithMessage("Code cannot exceed 256 characters");
    }
}
