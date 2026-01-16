namespace Valetax.Application.Common.Interfaces;

/// <summary>
/// Abstraction for getting current date/time. Useful for testing.
/// </summary>
public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
