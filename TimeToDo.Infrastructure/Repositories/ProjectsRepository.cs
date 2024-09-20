using Microsoft.EntityFrameworkCore;
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
}
