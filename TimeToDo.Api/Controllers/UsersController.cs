using Microsoft.AspNetCore.Mvc;
using TimeToDo.Application.Dtos;
using TimeToDo.Application.Queries;
using TimeToDo.Controllers;

namespace TimeToDo.API.Controllers;

public class UsersController : ApiController
{
    [HttpGet("User")]
    public async Task<ActionResult<UserDto>> GetUser(GetUserQuery request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);

        return Ok(result);
    }
}
