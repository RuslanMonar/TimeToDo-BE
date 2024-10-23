using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TimeToDo.Application.Interfaces.Infrastructure;
using TimeToDo.Application.Interfaces.Infrastructure.Repositories;
using TimeToDo.Domain.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

    public async Task<List<List<string>>> GetRecomendationsAsync(Guid userId)
    {
        var wellDoneRecommendations = new List<string>();
        var improvementRecommendations = new List<string>();
        var badDoneRecommendations = new List<string>();
        var mentalHealthRecommendations = new List<string>();

        //1. Порада по дню тижня:
        var bestDayOfWeek = await _dbContext.TaskSessions
            .Where(ts => ts.Task.UserId == userId && ts.IsFullItteration)
            .GroupBy(ts => ts.StartDate.DayOfWeek)
            .Select(g => new { Day = g.Key, CompletedCount = g.Count() })
            .OrderByDescending(g => g.CompletedCount)
            .FirstOrDefaultAsync();

        if (bestDayOfWeek != null)
        {
            wellDoneRecommendations.Add($"You most often make full itterations on {bestDayOfWeek.Day}. Try planning tasks for this day as you are more prodductive!");
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

            wellDoneRecommendations.Add($"You usually make full itterations between {productiveTime.Hour}:00 and {productiveTime.Hour + 1}:00. Try scheduling tasks for this time!");
        }

            //3. Порада по ітераціях:
            // Отримуємо кількість незавершених і завершених сесій одночасно
            var sessionCounts = await _dbContext.TaskSessions
                .Where(ts => ts.Task.UserId == userId)
                .GroupBy(ts => ts.IsFullItteration)
                .Select(g => new
                {
                    IsFullIteration = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            var incompleteSessions = sessionCounts.FirstOrDefault(sc => !sc.IsFullIteration)?.Count ?? 0;
            var completeSessions = sessionCounts.FirstOrDefault(sc => sc.IsFullIteration)?.Count ?? 0;
            var totalSessions = incompleteSessions + completeSessions;

            string recommendationMessage = string.Empty;

            var incompletePercentage = (double)incompleteSessions / totalSessions * 100;
            recommendationMessage += $"You have not completed {incompleteSessions} sessions ({incompletePercentage:F1}% of all sessions)." +
                $" And completed {completeSessions} full sessions.";

            if(incompletePercentage > 10 && incompletePercentage < 20)
            {
                recommendationMessage += "This is the acceptable limit";
                wellDoneRecommendations.Add(recommendationMessage);
            }

            if (incompletePercentage > 20 && incompletePercentage < 40)
            {
                recommendationMessage += "Your incomplete sessions suggest you're struggling with sustained focus. Try planning shorter iterations." +
                   "Break tasks into smaller chunks, " +
                   "set realistic goals, and ensure you're taking frequent breaks. Reflect on what might be causing distractions—perhaps try using ";

                improvementRecommendations.Add(recommendationMessage);
            }

        if (incompletePercentage >= 40)
        {
            badDoneRecommendations.Add("There are serious issues with maintaining focus, as more than 40% of your sessions are incomplete.Here are some strategies to help improve your focus:");

            badDoneRecommendations.Add("1. **Identify the root cause**: Evaluate whether it's external distractions, fatigue, or task complexity.");

            badDoneRecommendations.Add("2. **Iteration length**: Try planning shorter iterations.");

            badDoneRecommendations.Add("3. **Minimize distractions**: Turn off notifications, create a dedicated workspace, or use noise-cancelling headphones.");

            badDoneRecommendations.Add("4. **Physical health**: Stay hydrated, eat healthy snacks, and ensure you're getting enough rest. Fatigue can severely impact focus.");

            badDoneRecommendations.Add("5. **Mindfulness and relaxation**: Practice mindfulness techniques like deep breathing or short meditation sessions to regain focus and calm.");

            badDoneRecommendations.Add("6. **Accountability**: Share your goals with a colleague, friend, or coach who can help you stay on track.");

            badDoneRecommendations.Add("7. **Workspace optimization**: Adjust your lighting, desk, and chair setup to be more conducive to focused work.");

            badDoneRecommendations.Add("By addressing these areas, you can work towards reducing the number of incomplete sessions and improving your productivity.");
        }

        // Отримуємо загальну кількість задач користувача
        var totalTasks = await _dbContext.Tasks
            .Where(t => t.UserId == userId)
            .CountAsync();

        // 4. Порада по незавершеним задачам до EndDate
        var incompleteTasks = await _dbContext.Tasks
            .Where(t => t.UserId == userId && !t.IsCompleted && t.EndDate < DateTimeOffset.UtcNow.UtcDateTime)
            .ToListAsync();

        if (incompleteTasks.Any())
        {
            // Розраховуємо відсоток незавершених задач
            incompletePercentage = (double)incompleteTasks.Count / totalTasks * 100;
            recommendationMessage = $"You have {incompleteTasks.Count} tasks ({incompletePercentage:F1}% of all tasks) that are incomplete and were planned to be completed due before the current date.";

            // Додаємо рекомендації в залежності від відсотку незавершених задач
            if (incompletePercentage <= 10)
            {
                recommendationMessage += " This is within an acceptable limit. Great job on keeping up with most of your tasks!";
                wellDoneRecommendations.Add(recommendationMessage);
            }
            else if (incompletePercentage > 10 && incompletePercentage <= 30)
            {
                recommendationMessage += " It looks like some tasks slipped through. Consider reviewing your time management and breaking tasks into smaller steps to ensure deadlines are met.";
                improvementRecommendations.Add(recommendationMessage);
            }
            else if (incompletePercentage > 30 && incompletePercentage <= 50)
            {
                recommendationMessage += " A significant number of your tasks are overdue. Try prioritizing these tasks or adjusting your approach to deadlines to prevent task overload.";
                improvementRecommendations.Add(recommendationMessage);
            }
            else if (incompletePercentage > 50)
            {
                recommendationMessage += " More than 50% of your tasks are overdue. This could indicate a serious issue with your task management or planning. Here are some strategies:\n" +
                    "1. **Reevaluate Priorities**: Focus on the most urgent tasks first and reschedule others.\n" +
                    "2. **Break Tasks into Smaller Steps**: Complex tasks might be overwhelming, so break them down to make them more manageable.\n" +
                    "3. **Set Clear Deadlines**: Ensure that each task has a clear, realistic deadline and stick to it.\n" +
                    "4. **Review Your Workflow**: Analyze where the bottlenecks are in your workflow and make adjustments.\n" +
                    "5. **Take Regular Breaks**: Overworking can reduce productivity. Consider implementing regular short breaks to stay fresh and focused.";
                badDoneRecommendations.Add(recommendationMessage);
            }
        }

        // 5.
        recommendationMessage = string.Empty;
        var completedTasks = await _dbContext.Tasks
            .Where(t => t.UserId == userId && t.IsCompleted)
            .Select(t => new
            {
                TaskId = t.Id,
                IterationDuration = t.TomatoLenght // Припустимо, що є властивість IterationDuration
            })
            .ToListAsync(); // Отримуємо результати в пам'ять

        if (completedTasks.Any())
        {
            var taskSessionsCount = await _dbContext.TaskSessions
                .Where(ts => completedTasks.Select(ct => ct.TaskId).Contains(ts.TaskId))
                .GroupBy(ts => ts.TaskId)
                .Select(g => new
                {
                    TaskId = g.Key,
                    SessionCount = g.Count(),
                    TotalSessionsDuration = g.Sum(x => x.SessionDurationMinutes)
                })
                .ToListAsync();

            // Розрахунок середньої кількості ітерацій для всіх задач
            var totalIterations = 0;
            var totalDuration = 0;

            foreach (var task in taskSessionsCount)
            {
                totalIterations += task.SessionCount; // Множимо кількість сесій на тривалість ітерацій
                totalDuration += task.TotalSessionsDuration; // Сумуємо тривалість ітерацій
            }

            var averageSessionDuration = totalDuration / totalIterations; // Середня тривалість сесії
            var averageTimePerTask = totalDuration / completedTasks.Count(); // Середній час потрачений на задачу
            var averageSessionsPerTask = totalIterations / completedTasks.Count(); // Середня кількість сесій потрачених на задачу

            if (averageSessionDuration > 30)
            {
                recommendationMessage += $"Your average sessions ({averageSessionDuration} min) are longer than the optimal focus window of 25-30 minutes. Try shortening your sessions to maintain focus and avoid fatigue. ";
                improvementRecommendations.Add(recommendationMessage);
                recommendationMessage = string.Empty;
            }
            else if (averageSessionDuration < 15)
            {
                recommendationMessage += $"Your average sessions are relatively short (({averageSessionDuration} min)). Consider extending them to around 25-30 minutes for deeper concentration. ";
                improvementRecommendations.Add(recommendationMessage);
                recommendationMessage = string.Empty;
            }
            else if (averageSessionDuration >= 15 && averageSessionDuration <= 30) 
            {
                recommendationMessage += $"Your average sessions (({averageSessionDuration} min)). Good job !";
                wellDoneRecommendations.Add(recommendationMessage);
                recommendationMessage = string.Empty;
            }

            // Рекомендації по середньому часу на задачу
            if (averageTimePerTask > 120) // більше 2 годин на задачу
            {
                recommendationMessage += $"Avarage time per task = {ConvertMinutesToReadableTime(averageTimePerTask)}. You are spending a significant amount of time on each task. Try breaking down large tasks into smaller, manageable sub-tasks to better track your progress and reduce stress. ";
            }
            else if (averageTimePerTask < 30)
            {
                recommendationMessage += $"Avarage time per task = {ConvertMinutesToReadableTime(averageTimePerTask)}. You're completing tasks quickly. Ensure the speed doesn't compromise the quality of work.";
            }

            else if (averageSessionsPerTask < 2)
            {
                recommendationMessage += "You're finishing tasks quickly with minimal sessions, indicating efficient work. Keep up the good work! ";
            }

            improvementRecommendations.Add(recommendationMessage);
        }

        //6. Аналіз перевтоми:
        var longSessions = await _dbContext.TaskSessions
            .Where(ts => ts.Task.UserId == userId && ts.SessionDurationMinutes > 60)
            .CountAsync();

        if (longSessions > 5)
        {
            mentalHealthRecommendations.Add($"You had {longSessions} sessions longer than an hour. We recommend taking short breaks to avoid overwork.");
        }

        // 7. Перевірка, чи задачі з низьким пріоритетом виконуються частіше, ніж з високим:
        var priorityTaskCounts = await _dbContext.Tasks
            .Where(t => t.UserId == userId && t.IsCompleted)
            .GroupBy(t => t.Priority)
            .Select(g => new
            {
                Priority = g.Key,
                Count = g.Count()
            })
            .ToListAsync();

        var highPriorityTaskCount = priorityTaskCounts.FirstOrDefault(p => p.Priority == Priority.High)?.Count ?? 0;
        var mediumPriorityTaskCount = priorityTaskCounts.FirstOrDefault(p => p.Priority == Priority.Medium)?.Count ?? 0;
        var lowPriorityTaskCount = priorityTaskCounts.FirstOrDefault(p => p.Priority == Priority.Low)?.Count ?? 0;



        if (lowPriorityTaskCount > highPriorityTaskCount || mediumPriorityTaskCount > highPriorityTaskCount)
        {
            badDoneRecommendations.Add($"You are completing tasks with lower priority more frequently than high-priority tasks. " +
                                    "Consider focusing on high-priority tasks to ensure you're addressing the most critical items first.");
        }
        else if (highPriorityTaskCount > lowPriorityTaskCount)
        {
            wellDoneRecommendations.Add($"You are completing high-priority tasks appropriately. Keep up the good work!");
        }

        mentalHealthRecommendations.Add("You have a significant number of work sessions late at night.While night - time work can feel productive, it's essential to ensure you're getting enough sleep.A good night's rest enhances focus, memory, and creativity. Try to establish a healthy sleep routine to maintain your mental and physical well-being.");
        mentalHealthRecommendations.Add("You've been working for long sessions without breaks. While deep focus can be productive, taking regular breaks is crucial for mental health. Try the Pomodoro technique or taking short breaks every 25-30 minutes to refresh your mind and prevent burnout.");
        mentalHealthRecommendations.Add("You've been working during weekends. While it's great to stay productive, it's also important to give yourself time to rest. Consider setting aside time for relaxation and hobbies on weekends to recharge mentally.");


        return new List<List<string>>
        {
            wellDoneRecommendations,
            improvementRecommendations,
            badDoneRecommendations,
            mentalHealthRecommendations,
        };
    }
    public string ConvertMinutesToReadableTime(int minutes)
    {
        if (minutes >= 60)
        {
            int hours = minutes / 60;
            int remainingMinutes = minutes % 60;

            if (remainingMinutes > 0)
            {
                return $"{hours} hours and {remainingMinutes} minutes";
            }
            else
            {
                return $"{hours} hours";
            }
        }
        else
        {
            return $"{minutes} minutes";
        }
    }
}
