using AutoMapper;
using MediatR;
using TimeToDo.Application.Interfaces.Infrastructure.Repositories;
using TimeToDo.Shared;

namespace TimeToDo.Application.Queries;
public class GetRecomendationsQuery : IRequest<List<List<string>>>
{
}

public class GetRecomendationsQueryHandler : IRequestHandler<GetRecomendationsQuery, List<List<string>>>
{
    private readonly ITasksRepository _tasksReporistory;
    private readonly IRequestUser _requestUser;

    public GetRecomendationsQueryHandler(ITasksRepository tasksReporistory, IRequestUser requestUser)
    {
        _tasksReporistory = tasksReporistory;
        _requestUser = requestUser;
    }

    public async Task<List<List<string>>> Handle(GetRecomendationsQuery request, CancellationToken cancellationToken)
    {
        return await _tasksReporistory.GetRecomendationsAsync(_requestUser.Id);
    }
}