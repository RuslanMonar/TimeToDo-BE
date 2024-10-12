using MediatR;
using TimeToDo.Application.Interfaces.Infrastructure.Repositories;
using TimeToDo.Domain.Enums;
using TimeToDo.Shared;

namespace TimeToDo.Application.Commands;
public class UpdateTaskQuery : IRequest<Unit>
{
    public int TaskId { get; set; }
    public string Title { get; set; }
    public Priority Priority { get; set; }
    public int TomatoCount { get; set; }
    public int TomatoLenght { get; set; }
    public int ProjectId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Description { get; set; }
}

public class UpdateTaskQueryHandler : IRequestHandler<UpdateTaskQuery, Unit>
{
    private readonly ITasksRepository _tasksRepository;
    private readonly IRequestUser _requestUser;

    public UpdateTaskQueryHandler(ITasksRepository tasksRepository, IRequestUser requestUser)
    {
        _tasksRepository = tasksRepository;
        _requestUser = requestUser;

    }

    public async Task<Unit> Handle(UpdateTaskQuery request, CancellationToken cancellationToken)
    {
        // Get the current user ID
        var userId = _requestUser.Id;

        // Retrieve the task by taskId and userId
        var task = await _tasksRepository.GetTasksAsync(userId, request.ProjectId, request.TaskId, false, cancellationToken);

        // Ensure the task exists and belongs to the user
        var existingTask = task.FirstOrDefault();

        if (existingTask == null)
        {
            throw new KeyNotFoundException("Task not found or user does not have permission to update it.");
        }

        existingTask.Title = request.Title;
        existingTask.Priority = request.Priority;
        existingTask.TomatoCount = request.TomatoCount;
        existingTask.TomatoLenght = request.TomatoLenght;
        existingTask.ProjectId = request.ProjectId;
        existingTask.StartDate = request.StartDate;
        existingTask.EndDate = request.EndDate;
        existingTask.Description = request.Description;

        await _tasksRepository.UpdateTaskAsync(existingTask, existingTask.Id, userId);

        return Unit.Value;
    }
}
