using MediatR;

namespace Valetax.Application.Features.Auth.Commands.RememberMe;

/// <summary>
/// Command to authenticate a user by unique code and return a JWT token.
/// </summary>
public sealed record RememberMeCommand(string Code) : IRequest<TokenDto>;

/// <summary>
/// DTO for authentication token.
/// </summary>
public sealed record TokenDto(string Token);
