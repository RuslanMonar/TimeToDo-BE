using Microsoft.AspNetCore.Mvc;
using TimeToDo.Application.Commands;
using TimeToDo.Application.Dtos;
using TimeToDo.Application.Queries;
using TimeToDo.Controllers;

namespace TimeToDo.API.Controllers;

public class TasksController : ApiController
{
    [HttpPost]
    public async Task<ActionResult> CreateTask([FromBody] CreateTaskCommand command)
    {
        await Mediator.Send(command);

        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<List<TaskDto>>> GetTasks(GetTasksQuery request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet("GetTaskPerformance")]
    public async Task<ActionResult> GetTaskPerformance(GetTaskPerormanceStatisicQuery request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request, cancellationToken);
        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> UpdateTask([FromBody] UpdateTaskQuery request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request, cancellationToken);
        return Ok();
    }
    
    [HttpPost("CreateTaskSession")]
    public async Task<ActionResult> CreateTaskSessiond([FromBody] CreateTaskSessionCommand request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request, cancellationToken);
        return Ok();
    }
}
