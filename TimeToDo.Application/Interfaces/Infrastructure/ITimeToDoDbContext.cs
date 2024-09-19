namespace TimeToDo.Application.Interfaces.Infrastructure;
public interface ITimeToDoDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
