using Microsoft.AspNetCore.Mvc;
using Valetax.Application.Features.Auth.Commands.RememberMe;

namespace Valetax.API.Controllers;

/// <summary>
/// Controller for authentication operations.
/// </summary>
public sealed class PartnerController : ApiControllerBase
{
    /// <summary>
    /// Authenticates a user by unique code and returns a JWT token.
    /// </summary>
    /// <param name="code">The unique user code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The authentication token.</returns>
    [HttpPost("api.user.partner.rememberMe")]
    public async Task<ActionResult<TokenDto>> RememberMe(
        [FromQuery] string code,
        CancellationToken cancellationToken)
    {
        var command = new RememberMeCommand(code);
        var result = await Mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
