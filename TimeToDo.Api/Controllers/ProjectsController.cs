using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeToDo.Application.Commands;
using TimeToDo.Application.Dtos;
using TimeToDo.Application.Queries;
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

    [HttpGet]
    public async Task<ActionResult<List<ProjectDto>>> GetProjects(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("GetProjectsSatistic")]
    public async Task<ActionResult<List<ProjectStatisticsDto>>> GetProjectsSatistic(GetProjectStatisticQuery request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);
        return Ok(result);
    }
}
