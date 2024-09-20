using Microsoft.EntityFrameworkCore;
using TimeToDo.Domain.Entities;

namespace TimeToDo.Application.Interfaces.Infrastructure;
public interface ITimeToDoDbContext
{
    public DbSet<Folder> Folders { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Domain.Entities.Task> Tasks { get; set; }
    public DbSet<TaskSession> TaskSessions { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
