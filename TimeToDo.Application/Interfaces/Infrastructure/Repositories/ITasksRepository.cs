namespace TimeToDo.Application.Interfaces.Infrastructure.Repositories;
public interface ITasksRepository
{
    public Task CreateTaskAsync(Domain.Entities.Task task);
    public Task<List<Domain.Entities.Task>> GetTasksAsync(Guid userId, int? projectId, CancellationToken cancellationToken);
}
