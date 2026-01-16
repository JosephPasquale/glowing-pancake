using Valetax.Domain.Entities;

namespace Valetax.Application.Common.Interfaces;

/// <summary>
/// Interface for JWT token generation.
/// </summary>
public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
