using System.Security.Claims;
using Valetax.Application.Common.Interfaces;

namespace Valetax.API.Services;

/// <summary>
/// Implementation of ICurrentUserService for getting current user from HttpContext.
/// </summary>
public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public long? UserId
    {
        get
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("uid")?.Value;
            return long.TryParse(userIdClaim, out var userId) ? userId : null;
        }
    }

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
}
