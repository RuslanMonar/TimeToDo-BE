using Microsoft.EntityFrameworkCore;
using TimeToDo.Application.Interfaces.Infrastructure;
using TimeToDo.Application.Interfaces.Infrastructure.Repositories;
using TimeToDo.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace TimeToDo.Infrastructure.Repositories;
public class TaskSessionsRepository : ITaskSessionsRepository
{
    public readonly ITimeToDoDbContext _dbContext;
    public TaskSessionsRepository(ITimeToDoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<TaskSession>> GetTaskSessionsAsync(int taskId, CancellationToken cancellationToken)
    {
        return await _dbContext.TaskSessions.Where(x => x.TaskId == taskId).ToListAsync(cancellationToken);
    }

    public async Task CreateTaskSessionsAsync(TaskSession taskSession)
    {
        _dbContext.TaskSessions.Add(taskSession);
        await _dbContext.SaveChangesAsync();
    }
}
