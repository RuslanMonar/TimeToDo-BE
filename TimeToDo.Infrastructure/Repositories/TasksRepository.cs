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

    public async Task<List<Domain.Entities.Task>> GetTasksAsync(Guid userId, int? projectId, int? taskId, CancellationToken cancellationToken)
    {
        var tasksQuery = _dbContext.Tasks.Where(x => x.UserId == userId).AsQueryable();

        if(taskId != null)
        {
            tasksQuery = tasksQuery.Where(x => x.Id == taskId);
        }

        if(projectId != null)
        {
            tasksQuery = tasksQuery.Where(x => x.ProjectId == projectId);
        }

        tasksQuery = tasksQuery.Include(x => x.TaskSessions);

        return await tasksQuery.ToListAsync(cancellationToken);
    }

    public async Task UpdateTaskAsync(Domain.Entities.Task updatedTask, int taskId, Guid userId)
    {
        var task = await _dbContext.Tasks.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == taskId);

        if (task == null)
        {
            throw new KeyNotFoundException("Task not found.");
        }

        // Update task fields
        task.Title = updatedTask.Title;
        task.Priority = updatedTask.Priority;
        task.TomatoCount = updatedTask.TomatoCount;
        task.TomatoLenght = updatedTask.TomatoLenght;
        task.ProjectId = updatedTask.ProjectId;
        task.StartDate = updatedTask.StartDate;
        task.EndDate = updatedTask.EndDate;
        task.Description = updatedTask.Description;

        // Save changes
        await _dbContext.SaveChangesAsync();
    }
}
