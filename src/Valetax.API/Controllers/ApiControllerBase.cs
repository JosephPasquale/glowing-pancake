using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Valetax.API.Controllers;

/// <summary>
/// Base controller with common functionality.
/// </summary>
[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender? _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
