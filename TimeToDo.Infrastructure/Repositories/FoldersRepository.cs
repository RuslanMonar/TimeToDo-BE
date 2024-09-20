using Microsoft.EntityFrameworkCore;
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

    public async Task<List<Folder>> GetlFoldersAsync (Guid userId)
    {
        return await _dbContext.Folders.Where(x => x.UserId == userId).ToListAsync();
    }
}
