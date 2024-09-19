using TimeToDo.Application.Interfaces.Infrastructure;
using TimeToDo.Application.Interfaces.Infrastructure.Repositories;
using TimeToDo.Domain.Entities;

namespace TimeToDo.Infrastructure.Repositories;
public class FoldersRepository : IFoldersRepository
{
    public readonly ITimeToDoDbContext _dbContext;

    public FoldersRepository(ITimeToDoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async System.Threading.Tasks.Task CreateFolderAsync(Folder folder)
    {
        _dbContext.Folders.Add(folder);
        await _dbContext.SaveChangesAsync();
    }
}
