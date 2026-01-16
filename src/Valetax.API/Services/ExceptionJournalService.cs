using Valetax.Application.Common.Interfaces;
using Valetax.Domain.Entities;
using Valetax.Domain.Repositories;

namespace Valetax.API.Services;

public sealed class ExceptionJournalService : IExceptionJournalService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ExceptionJournalService(IServiceScopeFactory serviceScopeFactory, IDateTimeProvider dateTimeProvider)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<long> LogExceptionAsync(
        Exception exception,
        string requestPath,
        string queryParameters,
        string? bodyParameters,
        CancellationToken cancellationToken = default)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var journal = ExceptionJournal.Create(
            exceptionType: exception.GetType().Name,
            exceptionMessage: exception.Message,
            stackTrace: exception.StackTrace ?? string.Empty,
            requestPath: requestPath,
            queryParameters: queryParameters,
            bodyParameters: bodyParameters,
            createdAt: _dateTimeProvider.UtcNow);

        await unitOfWork.ExceptionJournals.AddAsync(journal, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return journal.EventId;
    }
}
