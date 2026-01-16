using FluentValidation;

namespace Valetax.Application.Features.Journal.Queries.GetJournalSingle;

/// <summary>
/// Validator for GetJournalSingleQuery.
/// </summary>
public sealed class GetJournalSingleQueryValidator : AbstractValidator<GetJournalSingleQuery>
{
    public GetJournalSingleQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Journal ID must be greater than 0");
    }
}
