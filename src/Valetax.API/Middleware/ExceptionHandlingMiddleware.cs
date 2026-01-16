using System.Globalization;
using System.Text.Json;
using Valetax.API.Models;
using Valetax.API.Services;
using Valetax.Domain.Exceptions;

namespace Valetax.API.Middleware;

public sealed partial class ExceptionHandlingMiddleware
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IExceptionJournalService journalService)
    {
        string? requestBody = null;

        try
        {
            context.Request.EnableBuffering();

            if (context.Request.ContentLength > 0 && context.Request.Body.CanSeek)
            {
                context.Request.Body.Position = 0;
                using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, journalService, requestBody);
        }
    }

    private async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception,
        IExceptionJournalService journalService,
        string? requestBody)
    {
        var requestPath = context.Request.Path.ToString();
        var queryParameters = context.Request.QueryString.ToString();

        var eventId = await journalService.LogExceptionAsync(
            exception,
            requestPath,
            queryParameters,
            requestBody,
            context.RequestAborted);

        var eventIdString = eventId.ToString(CultureInfo.InvariantCulture);

        LogException(exception, eventIdString);

        context.Response.ContentType = "application/json";

        ApiErrorResponse response;

        if (exception is SecureException secureException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            var exceptionTypeName = secureException.GetType().Name.Replace("Exception", string.Empty);
            response = new ApiErrorResponse(
                Type: exceptionTypeName,
                Id: eventIdString,
                Data: new ApiErrorData(Message: secureException.Message));
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            response = new ApiErrorResponse(
                Type: "Exception",
                Id: eventIdString,
                Data: new ApiErrorData(Message: $"Internal server error ID = {eventIdString}"));
        }

        var json = JsonSerializer.Serialize(response, JsonOptions);

        await context.Response.WriteAsync(json);
    }

    [LoggerMessage(Level = LogLevel.Error, Message = "Exception occurred. EventId: {EventId}")]
    private partial void LogException(Exception ex, string eventId);
}
