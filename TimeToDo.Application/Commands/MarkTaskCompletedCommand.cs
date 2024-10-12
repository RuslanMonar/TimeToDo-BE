using MediatR;
using TimeToDo.Application.Interfaces.Infrastructure.Repositories;
using TimeToDo.Shared;

namespace TimeToDo.Application.Commands;
public class MarkTaskCompletedCommand : IRequest<Unit>
{
    public int TaskId { get; set; }
    public bool Completed { get; set; }
    public DateTime? DateCompleted { get; set; }
}

public class MarkTaskCompletedCommandHandler : IRequestHandler<MarkTaskCompletedCommand, Unit>
{
    private readonly ITasksRepository _tasksRepository;
    private readonly IRequestUser _requestUser;

    public MarkTaskCompletedCommandHandler(ITasksRepository tasksRepository, IRequestUser requestUser)
    {
        _tasksRepository = tasksRepository;
        _requestUser = requestUser;

    }

    public async Task<Unit> Handle(MarkTaskCompletedCommand request, CancellationToken cancellationToken)
    {
        await _tasksRepository.MarkTaskCompletedAsync(request.TaskId, request.Completed, request.DateCompleted, _requestUser.Id);

        return Unit.Value;
    }
}
