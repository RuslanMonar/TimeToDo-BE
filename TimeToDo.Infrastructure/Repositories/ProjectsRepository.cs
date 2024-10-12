using Microsoft.EntityFrameworkCore;
using TimeToDo.Application.Dtos;
using TimeToDo.Application.Interfaces.Infrastructure;
using TimeToDo.Application.Interfaces.Infrastructure.Repositories;
using TimeToDo.Domain.Entities;

namespace TimeToDo.Infrastructure.Repositories;
public class ProjectsRepository : IProjectsRepository
{
    public readonly ITimeToDoDbContext _dbContext;
    public ProjectsRepository(ITimeToDoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Project> CreateProjectAsync(Project project)
    {
        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync();

        return project;
    }

    public async Task<List<Project>> GetProjectsAsync(int? folderId, Guid userId, CancellationToken cancellationToken)
    {
        var projectsQuery = _dbContext.Projects.Where(x => x.UserId == userId).AsQueryable();

        if (folderId != null)
        {
            projectsQuery = projectsQuery.Where(x => x.FolderId == folderId);
        }

        return await projectsQuery.ToListAsync(cancellationToken);
    }

    public async Task<List<ProjectStatisticsDto>> GetProjectsStatisticAsync(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Projects
        .Where(p => p.UserId == userId)
        .Select(p => new ProjectStatisticsDto
        {
            ProjectId = p.Id,
            ProjectTitle = p.Title,
            TotalHours = p.Tasks
                .SelectMany(t => t.TaskSessions)
                .Sum(ts => ts.SessionDurationMinutes) / 60,
        })
        .ToListAsync(cancellationToken);

        return result;
    }

    public async Task<List<ProjectSessionDto>> GetProjectsTimelineAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Projects
        .Where(p => p.UserId == userId)
        .Include(p => p.Tasks)
        .ThenInclude(t => t.TaskSessions)
        .Select(p => new ProjectSessionDto
        {
            Name = p.Title,
            Data = p.Tasks.SelectMany(t => t.TaskSessions.Select(ts => new TaskSessionDataDto
            {
                X = t.Title,
                Y = new string[]
                {
                    ts.StartDate.Add(ts.TimerStart).ToString("yyyy-MM-ddTHH:mm:ssZ"),  // Додаємо timerStart до startDate
                    ts.TimerEnd.HasValue
                        ? ts.StartDate.Add(ts.TimerEnd.Value).ToString("yyyy-MM-ddTHH:mm:ssZ")  // Якщо timerEnd не null
                        : ts.TimerPause.HasValue
                            ? ts.StartDate.Add(ts.TimerPause.Value).ToString("yyyy-MM-ddTHH:mm:ssZ")  // Якщо timerEnd null, то додаємо timerPause
                            : ts.StartDate.AddMinutes(ts.SessionDurationMinutes).ToString("yyyy-MM-ddTHH:mm:ssZ")  // Якщо обидва null, додаємо duration
                }
            })).ToList()
        }).ToListAsync();
    }

    public async Task<List<ProjectStatisticsTimeRangeDto>> GetProjectsStatisticTimeRangeAsync(Guid userId, CancellationToken cancellationToken)
    {
        var oneMonthAgo = DateTime.UtcNow.AddMonths(-1);

        // Спочатку отримуємо всі дані з бази без використання складних функцій
        var projectSessions = await _dbContext.Projects
            .Where(p => p.UserId == userId)
            .Select(p => new
            {
                ProjectId = p.Id,
                ProjectTitle = p.Title,
                Sessions = p.Tasks
                    .SelectMany(t => t.TaskSessions)
                    .Where(ts => ts.StartDate >= oneMonthAgo) // Фільтруємо сесії за останній місяць
                    .Select(ts => new { ts.StartDate, ts.SessionDurationMinutes }) // Вибираємо тільки потрібні дані
            })
            .ToListAsync(cancellationToken);


        var result = projectSessions
            .Select(p => new ProjectStatisticsTimeRangeDto
            {
                Name = p.ProjectTitle,
                Data = p.Sessions
                    .GroupBy(ts => GetWeekOfYear(ts.StartDate))
                    .OrderBy(g => g.Key)
                    .Select(g => g.Sum(ts => ts.SessionDurationMinutes) / 60)
                    .ToList(),
                WeekStartDates = p.Sessions
                .GroupBy(ts => GetWeekOfYear(ts.StartDate)) 
                .OrderBy(g => g.Key)
                .Select(g => GetWeekStartDate(g.First().StartDate))
                .ToList()
            })
            .Where(p => p.Data.Any(hours => hours > 0))
            .ToList();

        return result;
    }

    private static int GetWeekOfYear(DateTime date)
    {
        var culture = System.Globalization.CultureInfo.CurrentCulture;
        return culture.Calendar.GetWeekOfYear(date, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
    }

    private static DateTime GetWeekStartDate(DateTime date)
    {
        var diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
        return date.AddDays(-1 * diff).Date; // Повертаємо початок тижня (понеділок)
    }
}
