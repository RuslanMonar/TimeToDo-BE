using AutoMapper;
using MediatR;
using TimeToDo.Application.Dtos;
using TimeToDo.Application.Interfaces.Infrastructure.Repositories;
using TimeToDo.Shared;

namespace TimeToDo.Application.Queries;
public class GetProjectsQuery : IRequest<List<ProjectDto>>
{
    public int? FolderId { get; set; }
}

public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, List<ProjectDto>>
{
    private readonly IProjectsRepository _projectsRepository;
    private readonly IRequestUser _requestUser;
    private readonly IMapper _mapper;

    public GetProjectsQueryHandler(IProjectsRepository projectsRepository, IRequestUser requestUser, IMapper mapper)
    {
        _projectsRepository = projectsRepository;
        _requestUser = requestUser;
        _mapper = mapper;

    }
    public async Task<List<ProjectDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await _projectsRepository.GetProjectsAsync(request.FolderId, _requestUser.Id, cancellationToken);
        return _mapper.Map<List<ProjectDto>>(projects);
    }
}
