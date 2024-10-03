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
    private readonly IMapper _mapper;

    public GetProjectStatisticQueryHandler(IProjectsRepository projectsRepository,
        IRequestUser requestUser, IMapper mapper)
    {
        _projectsRepository = projectsRepository;
        _requestUser = requestUser;
        _mapper = mapper;

    }

    public async Task<List<ProjectStatisticsDto>> Handle(GetProjectStatisticQuery request, CancellationToken cancellationToken)
    {
        var userId = new Guid("a2165ef3-d303-425b-be23-5e6ec8f039d6");
        var reslut = await _projectsRepository.GetProjectsStatisticAsync(userId, cancellationToken);
        return reslut.Where(x => x.TotalHours > 0).ToList();
    }
}
