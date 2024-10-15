using Microsoft.EntityFrameworkCore;
using TimeToDo.Application.Interfaces.Infrastructure;
using TimeToDo.Application.Interfaces.Infrastructure.Repositories;
using TimeToDo.Domain.Enums;

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

    public async Task<List<Domain.Entities.Task>> GetTasksAsync(Guid userId, int? projectId, int? taskId, bool completed, CancellationToken cancellationToken)
    {
        var tasksQuery = _dbContext.Tasks.Where(x => x.UserId == userId && x.IsCompleted == completed).AsQueryable();

        if (taskId != null)
        {
            tasksQuery = tasksQuery.Where(x => x.Id == taskId);
        }

        if (projectId != null)
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

        await _dbContext.SaveChangesAsync();
    }

    public async Task MarkTaskCompletedAsync(int taskId, bool completed, DateTime? dateCompleted, Guid userId)
    {
        var task = await _dbContext.Tasks.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == taskId);

        if (task == null)
        {
            throw new KeyNotFoundException("Task not found.");
        }

        task.IsCompleted = completed;
        task.DateCompleted = dateCompleted;

        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<string>> GetRecomendationsAsync(Guid userId)
    {
        var recomandations = new List<string>();

        //1. Порада по дню тижня:
        var bestDayOfWeek = await _dbContext.TaskSessions
            .Where(ts => ts.Task.UserId == userId && ts.IsFullItteration)
            .GroupBy(ts => ts.StartDate.DayOfWeek)
            .Select(g => new { Day = g.Key, CompletedCount = g.Count() })
            .OrderByDescending(g => g.CompletedCount)
            .FirstOrDefaultAsync();

        if (bestDayOfWeek != null)
        {
            recomandations.Add($"Ви найчастіше завершуєте задачі у {bestDayOfWeek.Day}. Спробуйте планувати задачі на цей день!");
        }

        //2. Порада по часу доби:
        var productiveTime = await _dbContext.TaskSessions
            .Where(ts => ts.Task.UserId == userId && ts.IsFullItteration)
            .GroupBy(ts => ts.TimerStart.Hours)
            .Select(g => new { Hour = g.Key, CompletedCount = g.Count() })
            .OrderByDescending(g => g.CompletedCount)
            .FirstOrDefaultAsync();

        if (productiveTime != null)
        {
            recomandations.Add($"Найчастіше ви завершуєте задачі між {productiveTime.Hour}:00 і {productiveTime.Hour + 1}:00. Спробуйте планувати задачі на цей час!");
        }

        //3. Порада по ітераціях:
        var incompleteSessions = await _dbContext.TaskSessions
        .Where(ts => ts.Task.UserId == userId && !ts.IsFullItteration)
        .CountAsync();

        if (incompleteSessions > 5)
        {
            recomandations.Add($"Ви не завершили {incompleteSessions} сесій. Спробуйте планувати коротші ітерації.");
        }

        //4. Порада по завершеним задачам:
        var longPendingTasks = await _dbContext.Tasks
    .Where(t => t.UserId == userId && t.IsCompleted &&
        t.StartDate >= DateTimeOffset.UtcNow.UtcDateTime.AddDays(-7))
    .ToListAsync();

        if (longPendingTasks.Any())
        {
            recomandations.Add($"У вас є {longPendingTasks.Count} задачі, які залишаються незавершеними більше тижня. Спробуйте розбити їх на менші задачі або встановити дедлайн.");
        }

        //5. Рекомендація по кількості ітерацій:
        var taskWithSessions = await _dbContext.TaskSessions
            .Where(ts => ts.Task.UserId == userId && ts.IsFullItteration)
            .GroupBy(ts => ts.TaskId)
            .Select(g => new { TaskId = g.Key, SessionCount = g.Count() })
            .OrderByDescending(g => g.SessionCount)
            .FirstOrDefaultAsync();

        if (taskWithSessions != null && taskWithSessions.SessionCount > 3)
        {
            recomandations.Add($"Задачі, що мали більше трьох ітерацій, частіше завершувались успішно. Спробуйте більше коротких сесій.");
        }

        //6. Аналіз перевтоми:
        var longSessions = await _dbContext.TaskSessions
            .Where(ts => ts.Task.UserId == userId && ts.SessionDurationMinutes > 60)
            .CountAsync();

        if (longSessions > 5)
        {
            recomandations.Add($"У вас було {longSessions} сесій більше години. Рекомендуємо робити короткі перерви, щоб уникнути перевтоми.");
        }

        //7. Рекомендація по часу для важливих задач:
        var highPriorityTasks = await _dbContext.Tasks
            .Where(t => t.UserId == userId && t.Priority == Priority.High && t.IsCompleted)
            .GroupBy(t => t.DateCompleted.Value.Hour)
            .Select(g => new { Hour = g.Key, Count = g.Count() })
            .OrderByDescending(g => g.Count)
            .FirstOrDefaultAsync();

        if (highPriorityTasks != null)
        {
            recomandations.Add($"Ви зазвичай завершували задачі з високим пріоритетом між {highPriorityTasks.Hour}:00 і {highPriorityTasks.Hour + 1}:00. Плануйте важливі задачі на цей час.");
        }

        return recomandations;
    }
}
