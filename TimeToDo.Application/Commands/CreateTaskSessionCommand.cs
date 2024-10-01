using AutoMapper;
using MediatR;
using TimeToDo.Application.Interfaces.Infrastructure.Repositories;
using TimeToDo.Domain.Entities;
using TimeToDo.Shared;

namespace TimeToDo.Application.Commands;
public class CreateTaskSessionCommand : IRequest<Unit>
{
    public int TaskId { get; set; }
    public Guid SessionId { get; set; }
    public DateTime StartDate { get; set; }
    public TimeSpan TimerStart { get; set; }
    public TimeSpan? TimerPause { get; set; }
    public TimeSpan? TimerEnd { get; set; }
    public int SessionDurationMinutes { get; set; }
    public bool IsFullItteration { get; set; }
}

public class CreateTaskSessionCommandHandler : IRequestHandler<CreateTaskSessionCommand, Unit>
{
    private readonly ITaskSessionsRepository _tasksSessionRepository;
    private readonly IRequestUser _requestUser;
    public readonly IMapper _mapper;

    public CreateTaskSessionCommandHandler(ITaskSessionsRepository tasksSessionRepository, IRequestUser requestUser, IMapper mapper)
    {
        _tasksSessionRepository = tasksSessionRepository;
        _requestUser = requestUser;
        _mapper = mapper;
    }


    public async Task<Unit> Handle(CreateTaskSessionCommand request, CancellationToken cancellationToken)
    {
        var taskSession = _mapper.Map<TaskSession>(request);
        await _tasksSessionRepository.CreateTaskSessionsAsync(taskSession);

        return Unit.Value;
    }
}
