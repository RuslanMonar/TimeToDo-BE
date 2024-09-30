using Microsoft.EntityFrameworkCore;
using TimeToDo.Application.Interfaces.Infrastructure;
using TimeToDo.Application.Interfaces.Infrastructure.Repositories;

namespace TimeToDo.Infrastructure.Repositories;
public class TasksRepository : ITasksRepository
{
    public readonly ITimeToDoDbContext _dbContext;
    public TasksRepository(ITimeToDoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateTaskAsync(Domain.Entities.Task task)
    {
        _dbContext.Tasks.Add(task);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Domain.Entities.Task>> GetTasksAsync(Guid userId, int? projectId, CancellationToken cancellationToken)
    {
        var tasksQuery = _dbContext.Tasks.Where(x => x.UserId == userId).AsQueryable();

        if(projectId != null)
        {
            tasksQuery = tasksQuery.Where(x => x.ProjectId == projectId);
        }
        tasksQuery = tasksQuery.Include(x => x.TaskSessions);

        return await tasksQuery.ToListAsync(cancellationToken);
    }
}
