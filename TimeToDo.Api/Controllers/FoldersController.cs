using Microsoft.AspNetCore.Mvc;
using TimeToDo.Application.Commands;
using TimeToDo.Application.Dtos;
using TimeToDo.Application.Queries;
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
    
    [HttpGet]
    public async Task<ActionResult<List<FolderDto>>> GetFolders(GetFoldersQuery request)
    {
        var result = await Mediator.Send(request);
        return Ok(result);
    }
}
