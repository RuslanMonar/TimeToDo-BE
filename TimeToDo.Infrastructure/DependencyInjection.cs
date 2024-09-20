using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TimeToDo.Application.Interfaces.Infrastructure;
using TimeToDo.Application.Interfaces.Infrastructure.Repositories;
using TimeToDo.Domain.Entities;
using TimeToDo.Infrastructure.Data;
using TimeToDo.Infrastructure.Repositories;

namespace TimeToDo.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TimeToDoDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<TimeToDoDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<ITimeToDoDbContext, TimeToDoDbContext>();
        services.AddScoped<IProjectsRepository, ProjectsRepository>();
        services.AddScoped<IFoldersRepository, FoldersRepository>();
        services.AddScoped<ITasksRepository, TasksRepository>();

        return services;
    }
}
