using MediatR;
using TimeToDo.Application.Dtos;
using TimeToDo.Application.Interfaces.Infrastructure.Repositories;
using TimeToDo.Shared;

namespace TimeToDo.Application.Queries;
public class GetProjectsStatisticTimeRangeQuery : IRequest<List<ProjectStatisticsTimeRangeDto>>
{
}

public class GetProjectsStatisticTimeRangeQueryHandler : IRequestHandler<GetProjectsStatisticTimeRangeQuery, List<ProjectStatisticsTimeRangeDto>>
{
    private readonly IProjectsRepository _projectsRepository;
    private readonly IRequestUser _requestUser;

    public GetProjectsStatisticTimeRangeQueryHandler(IProjectsRepository projectsRepository,
        IRequestUser requestUser)
    {
        _projectsRepository = projectsRepository;
        _requestUser = requestUser;
    }

    public async Task<List<ProjectStatisticsTimeRangeDto>> Handle(GetProjectsStatisticTimeRangeQuery request, CancellationToken cancellationToken)
    {
        return await _projectsRepository.GetProjectsStatisticTimeRangeAsync(_requestUser.Id, cancellationToken);
    }
}
