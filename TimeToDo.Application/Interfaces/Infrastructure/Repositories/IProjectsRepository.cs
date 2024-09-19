using TimeToDo.Domain.Entities;

namespace TimeToDo.Application.Interfaces.Infrastructure.Repositories;
public interface IProjectsRepository
{
    public Task<List<Project>> GetProjectsAsync(int? folderId, Guid userId, CancellationToken cancellationToken);
    public Task<Project> CreateProjectAsync(Project project);
}
