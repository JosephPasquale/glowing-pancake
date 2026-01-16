namespace Valetax.Application.Features.Journal.Queries.GetJournalRange;

/// <summary>
/// DTO for filtering journal entries.
/// </summary>
public sealed record JournalFilterDto(
    DateTime? From,
    DateTime? To,
    string? Search);
