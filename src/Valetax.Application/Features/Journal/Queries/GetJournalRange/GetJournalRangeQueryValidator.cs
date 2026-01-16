using FluentValidation;

namespace Valetax.Application.Features.Journal.Queries.GetJournalRange;

/// <summary>
/// Validator for GetJournalRangeQuery.
/// </summary>
public sealed class GetJournalRangeQueryValidator : AbstractValidator<GetJournalRangeQuery>
{
    public GetJournalRangeQueryValidator()
    {
        RuleFor(x => x.Skip)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Skip must be non-negative");

        RuleFor(x => x.Take)
            .GreaterThan(0)
            .WithMessage("Take must be greater than 0")
            .LessThanOrEqualTo(1000)
            .WithMessage("Take cannot exceed 1000");
    }
}
