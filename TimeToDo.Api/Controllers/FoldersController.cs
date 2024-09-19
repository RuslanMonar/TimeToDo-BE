using Microsoft.AspNetCore.Mvc;
using TimeToDo.Application.Commands;
using TimeToDo.Controllers;

namespace TimeToDo.API.Controllers;

public class FoldersController : ApiController
{
    [HttpPost]
    public async Task<ActionResult> CreateFolder([FromBody] CreateFolderCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }
}
