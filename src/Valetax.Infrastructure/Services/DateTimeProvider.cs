using Valetax.Application.Common.Interfaces;

namespace Valetax.Infrastructure.Services;

/// <summary>
/// Implementation of IDateTimeProvider.
/// </summary>
public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
