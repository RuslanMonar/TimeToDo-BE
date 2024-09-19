using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TimeToDo.Controllers;

[Route("api/timetodo/[controller]")]
public class ApiController : ControllerBase
{
    private IMediator _mediator;

    protected IMediator Mediator => (_mediator ??= HttpContext.RequestServices.GetService<IMediator>()!)!;
}

