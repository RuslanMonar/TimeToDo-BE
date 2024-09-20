using MediatR;
using TimeToDo.Application.Interfaces.Infrastructure.Repositories;
using TimeToDo.Domain.Enums;
using TimeToDo.Shared;

namespace TimeToDo.Application.Commands;

public class CreateTaskCommand : IRequest<Unit>
{
    public string Title { get; set; }
    public Priority Priority { get; set; }
    public int TomatoCount { get; set; }
    public int TomatoLenght { get; set; }
    public int ProjectId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Description { get; set; }

}

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Unit>
{
    private readonly ITasksRepository _tasksRepository;
    private readonly IRequestUser _requestUser;

    public CreateTaskCommandHandler(ITasksRepository tasksRepository, IRequestUser requestUser)
    {
        _tasksRepository = tasksRepository;
        _requestUser = requestUser;
    }

    public async Task<Unit> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = new Domain.Entities.Task()
        {
            Title = request.Title,
            Priority = request.Priority,
            TomatoCount = request.TomatoCount,
            TomatoLenght = request.TomatoLenght,
            ProjectId = request.ProjectId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Description = request.Description,
            IsCompleted = false,
            DateCompleted = null,
            UserId = _requestUser.Id
        };

        await _tasksRepository.CreateTaskAsync(task);

        return Unit.Value;
    }
}
