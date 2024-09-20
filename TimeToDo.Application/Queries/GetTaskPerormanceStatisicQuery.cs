using MediatR;
using TimeToDo.Application.Interfaces.Infrastructure.Repositories;

namespace TimeToDo.Application.Queries;
public class GetTaskPerormanceStatisicQuery : IRequest<Unit>
{
}

public class GetTaskPerormanceStatisicQueryHandler : IRequestHandler<GetTaskPerormanceStatisicQuery, Unit>
{
    private readonly ITaskSessionsRepository _taskSessionsRepository;

    public GetTaskPerormanceStatisicQueryHandler(ITaskSessionsRepository taskSessionsRepository)
    {
        _taskSessionsRepository = taskSessionsRepository;
    }

    public async Task<Unit> Handle(GetTaskPerormanceStatisicQuery request, CancellationToken cancellationToken)
    {
        var taskId = 1;

        var taskSessions = await _taskSessionsRepository.GetTaskSessionsAsync(taskId, cancellationToken);

        var groupedBySession = taskSessions.GroupBy(x => x.SessionId);
        //var timerPaused = groupedBySession.Select(x => x.Where(x => x.TimerPause != null).ToList()).ToList();
        var timerPaused = groupedBySession
            .Select(group => new
            {
                SessionId = group.Key,
                Count = group.Count(x => x.TimerPause != null),
                Date = group.Select(x => x.StartDate).FirstOrDefault(),
            })
            .Where(x => x.Count > 0)
            .ToList();

        return Unit.Value;
    }
}
