using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Valetax.Application.Common.Behaviors;

/// <summary>
/// MediatR pipeline behavior for logging request handling.
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The response type.</typeparam>
public sealed partial class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        LogHandling(requestName);

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var response = await next(cancellationToken);
            stopwatch.Stop();

            LogHandled(requestName, stopwatch.ElapsedMilliseconds);

            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            LogError(ex, requestName, stopwatch.ElapsedMilliseconds);

            throw;
        }
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Handling {RequestName}")]
    private partial void LogHandling(string requestName);

    [LoggerMessage(Level = LogLevel.Information, Message = "Handled {RequestName} in {ElapsedMilliseconds}ms")]
    private partial void LogHandled(string requestName, long elapsedMilliseconds);

    [LoggerMessage(Level = LogLevel.Error, Message = "Error handling {RequestName} after {ElapsedMilliseconds}ms")]
    private partial void LogError(Exception ex, string requestName, long elapsedMilliseconds);
}
