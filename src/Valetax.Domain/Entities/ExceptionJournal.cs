using Valetax.Domain.Common;

namespace Valetax.Domain.Entities;

public sealed class ExceptionJournal : AggregateRoot, IAuditableEntity
{
    public long EventId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ModifiedAt { get; private set; }
    public string ExceptionType { get; private set; } = null!;
    public string ExceptionMessage { get; private set; } = null!;
    public string StackTrace { get; private set; } = null!;
    public string RequestPath { get; private set; } = null!;
    public string QueryParameters { get; private set; } = null!;
    public string? BodyParameters { get; private set; }

    private ExceptionJournal()
    {
    }

    public static ExceptionJournal Create(
        string exceptionType,
        string exceptionMessage,
        string stackTrace,
        string requestPath,
        string queryParameters,
        string? bodyParameters,
        DateTime createdAt)
    {
        return new ExceptionJournal
        {
            EventId = createdAt.Ticks,
            CreatedAt = createdAt,
            ExceptionType = exceptionType,
            ExceptionMessage = exceptionMessage,
            StackTrace = stackTrace,
            RequestPath = requestPath,
            QueryParameters = queryParameters,
            BodyParameters = bodyParameters
        };
    }
}
