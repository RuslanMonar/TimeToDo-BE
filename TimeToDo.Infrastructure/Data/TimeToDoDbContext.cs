using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TimeToDo.Application.Interfaces.Infrastructure;
using TimeToDo.Domain.Entities;

namespace TimeToDo.Infrastructure.Data;
public class TimeToDoDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>, ITimeToDoDbContext
{
    public DbSet<Folder> Folders { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Domain.Entities.Task> Tasks { get; set; }
    public DbSet<TaskSession> TaskSessions { get; set; }

    public TimeToDoDbContext(DbContextOptions<TimeToDoDbContext> options) : base(options)
    {
    }
}
