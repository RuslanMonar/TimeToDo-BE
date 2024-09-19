using Microsoft.AspNetCore.Mvc;
using TimeToDo.Application.Commands;
using TimeToDo.Controllers;
using TimeToDo.Domain.Entities;

namespace TimeToDo.API.Controllers;

public class ProjectsController : ApiController
{
    [HttpPost]
    public async Task<ActionResult<Project>> CreateProject([FromBody] CreateProjectCommand command)
    {
        var project = await Mediator.Send(command);

        return Ok(project);
    }
}
