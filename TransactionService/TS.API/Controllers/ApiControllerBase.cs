using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TS.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public abstract class ApiControllerBase : ControllerBase
{
    private IMediator _mediatR = null!;
    protected IMediator Mediator => _mediatR ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

}
