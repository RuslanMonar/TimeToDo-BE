using AutoMapper;
using MediatR;
using TimeToDo.Application.Dtos;
using TimeToDo.Application.Interfaces.Infrastructure.Repositories;
using TimeToDo.Shared;

namespace TimeToDo.Application.Queries;

public class GetTasksQuery : IRequest<List<TaskDto>>
{
    public int? ProjectId { get; set; }
}

public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, List<TaskDto>>
{
    private readonly ITasksRepository _tasksReporistory;
    private readonly IRequestUser _requestUser;
    private readonly IMapper _mapper;

    public GetTasksQueryHandler(ITasksRepository tasksReporistory, IRequestUser requestUser, IMapper mapper)
    {
        _tasksReporistory = tasksReporistory;
        _requestUser = requestUser;
        _mapper = mapper;

    }
    public async Task<List<TaskDto>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _tasksReporistory.GetTasksAsync(_requestUser.Id, request.ProjectId, cancellationToken);

        foreach (var task in tasks)
        {
            var a = task.TaskSessions.Sum(s => s.SessionDurationMinutes);
        }

        return _mapper.Map<List<TaskDto>>(tasks);
    }
}
