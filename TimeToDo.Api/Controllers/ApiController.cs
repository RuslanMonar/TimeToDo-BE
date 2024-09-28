using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TimeToDo.Controllers;

[Authorize]
[Route("api/[controller]")]
public class ApiController : ControllerBase
{
    private IMediator _mediator;

    protected IMediator Mediator => (_mediator ??= HttpContext.RequestServices.GetService<IMediator>()!)!;
}

