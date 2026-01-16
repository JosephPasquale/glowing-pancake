namespace Valetax.Application.Features.Journal.Queries.GetJournalSingle;

/// <summary>
/// DTO for full journal entry details.
/// </summary>
public sealed record JournalDto(
    long Id,
    long EventId,
    DateTime CreatedAt,
    string Text);
