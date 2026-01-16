namespace Valetax.API.Services;

/// <summary>
/// Service for logging exceptions to the journal.
/// </summary>
public interface IExceptionJournalService
{
    Task<long> LogExceptionAsync(
        Exception exception,
        string requestPath,
        string queryParameters,
        string? bodyParameters,
        CancellationToken cancellationToken = default);
}
