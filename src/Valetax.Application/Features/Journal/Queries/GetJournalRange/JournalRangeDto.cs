namespace Valetax.Application.Features.Journal.Queries.GetJournalRange;

/// <summary>
/// DTO for paginated journal entries.
/// </summary>
public sealed record JournalRangeDto(
    int Skip,
    int Count,
    IReadOnlyList<JournalInfoDto> Items);
