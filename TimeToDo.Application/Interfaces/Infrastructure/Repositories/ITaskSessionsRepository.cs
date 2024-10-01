using TimeToDo.Domain.Entities;

namespace TimeToDo.Application.Interfaces.Infrastructure.Repositories;
public interface ITaskSessionsRepository
{
    public Task<List<TaskSession>> GetTaskSessionsAsync(int taskId, CancellationToken cancellationToken);
    System.Threading.Tasks.Task CreateTaskSessionsAsync(TaskSession taskSession);
}
