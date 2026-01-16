namespace Valetax.Application.Features.Journal.Queries.GetJournalRange;

/// <summary>
/// DTO for journal entry info (without full details).
/// </summary>
public sealed record JournalInfoDto(
    long Id,
    long EventId,
    DateTime CreatedAt);
