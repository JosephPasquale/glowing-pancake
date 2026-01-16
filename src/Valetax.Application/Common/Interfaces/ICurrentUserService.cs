namespace Valetax.Application.Common.Interfaces;

/// <summary>
/// Interface for getting information about the current user.
/// </summary>
public interface ICurrentUserService
{
    long? UserId { get; }
    bool IsAuthenticated { get; }
}
