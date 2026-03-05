using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Reservio.WebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public abstract class BaseApiController : ControllerBase {
	private IMediator _mediator = null!;
	protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
}

