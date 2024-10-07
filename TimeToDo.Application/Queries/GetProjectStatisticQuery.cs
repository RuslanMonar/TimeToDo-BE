using AutoMapper;
using MediatR;
using TimeToDo.Application.Dtos;
using TimeToDo.Application.Interfaces.Infrastructure.Repositories;
using TimeToDo.Shared;

namespace TimeToDo.Application.Queries;
public class GetProjectStatisticQuery : IRequest<List<ProjectStatisticsDto>>
{
}

public class GetProjectStatisticQueryHandler : IRequestHandler<GetProjectStatisticQuery, List<ProjectStatisticsDto>>
{
    private readonly IProjectsRepository _projectsRepository;
    private readonly IRequestUser _requestUser;

    public GetProjectStatisticQueryHandler(IProjectsRepository projectsRepository,
        IRequestUser requestUser)
    {
        _projectsRepository = projectsRepository;
        _requestUser = requestUser;
    }

    public async Task<List<ProjectStatisticsDto>> Handle(GetProjectStatisticQuery request, CancellationToken cancellationToken)
    {
        var userId = _requestUser.Id;
        var reslut = await _projectsRepository.GetProjectsStatisticAsync(userId, cancellationToken);
        return reslut.Where(x => x.TotalHours > 0).ToList();
    }
}
