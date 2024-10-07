using TimeToDo.Application.Dtos;
using TimeToDo.Domain.Entities;

namespace TimeToDo.Application.Interfaces.Infrastructure.Repositories;
public interface IProjectsRepository
{
    public Task<List<Project>> GetProjectsAsync(int? folderId, Guid userId, CancellationToken cancellationToken);
    public Task<Project> CreateProjectAsync(Project project);
    Task<List<ProjectStatisticsDto>> GetProjectsStatisticAsync(Guid userId, CancellationToken cancellationToken);
    Task<List<ProjectSessionDto>> GetProjectsTimelineAsync(Guid userId, CancellationToken cancellationToken);
}
