using MediatR;
using TimeToDo.Application.Dtos;
using TimeToDo.Application.Interfaces.Infrastructure.Repositories;
using TimeToDo.Shared;

namespace TimeToDo.Application.Queries;
public class GetProjectsTimelineQuery : IRequest<List<ProjectSessionDto>>
{
}

public class GetProjectsTimelineQueryHandler : IRequestHandler<GetProjectsTimelineQuery, List<ProjectSessionDto>>
{
    private readonly IProjectsRepository _projectsRepository;
    private readonly IRequestUser _requestUser;

    public GetProjectsTimelineQueryHandler(IProjectsRepository projectsRepository,
        IRequestUser requestUser)
    {
        _projectsRepository = projectsRepository;
        _requestUser = requestUser;
    }

    public async Task<List<ProjectSessionDto>> Handle(GetProjectsTimelineQuery request, CancellationToken cancellationToken)
    {
        return await _projectsRepository.GetProjectsTimelineAsync(_requestUser.Id, cancellationToken);
    }
}
