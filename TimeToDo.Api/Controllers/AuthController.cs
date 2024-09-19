using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeToDo.Application.Commands;
using TimeToDo.Application.Queries;
using TimeToDo.Controllers;
using TimeToDo.Domain.Models;

namespace TimeToDo.API.Controllers;

public class AuthController : ApiController
{
    [AllowAnonymous]
    [HttpPost("SignUp")]
    public async Task<ActionResult<AuthResult>> SignUp([FromBody]  SignUpCommand request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("SignIn")]
    public async Task<ActionResult<AuthResult>> SignIn([FromBody] SignInQuery request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);

        return Ok(result);
    }
}
