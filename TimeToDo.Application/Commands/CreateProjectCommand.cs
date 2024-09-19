using MediatR;
using TimeToDo.Application.Interfaces.Infrastructure.Repositories;
using TimeToDo.Domain.Entities;
using TimeToDo.Shared;

namespace TimeToDo.Application.Commands;
public class CreateProjectCommand : IRequest<Project>
{
    public string Title { get; set; }
    public int FolderId { get; set; }
    public string Color { get; set; }
}

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Project>
{
    private readonly IProjectsRepository _projectsRepository;
    private readonly IRequestUser _requestUser;

    public CreateProjectCommandHandler(IProjectsRepository projectsRepository, IRequestUser requestUser)
    {
        _projectsRepository = projectsRepository;
        _requestUser = requestUser;
    }

    public async Task<Project> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        //TODO:Check if user has access to folder from request, use automapper
        var project = new Project()
        {
            UserId = _requestUser.Id,
            Title = request.Title,
            FolderId = request.FolderId,
            Color = request.Color,
        };

        var result = await _projectsRepository.CreateProjectAsync(project);

        return result;
    }
}
