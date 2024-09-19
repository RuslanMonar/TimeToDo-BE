using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TimeToDo.Application.Interfaces.Infrastructure;
using TimeToDo.Domain.Entities;

namespace TimeToDo.Infrastructure.Data;
public class TimeToDoDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>, ITimeToDoDbContext
{
    public TimeToDoDbContext(DbContextOptions<TimeToDoDbContext> options) : base(options)
    {
    }
}
