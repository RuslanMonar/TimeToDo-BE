﻿namespace TimeToDo.Application.Interfaces.Infrastructure.Repositories;
public interface ITasksRepository
{
    public Task CreateTaskAsync(Domain.Entities.Task task);
    public Task<List<Domain.Entities.Task>> GetTasksAsync(Guid userId, int? projectId, int? taskId, bool completed, CancellationToken cancellationToken);
    public Task UpdateTaskAsync(Domain.Entities.Task task, int taskId, Guid userId);
}
