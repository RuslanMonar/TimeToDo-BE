using TimeToDo.Domain.Entities;

namespace TimeToDo.Application.Interfaces.Infrastructure.Repositories;
public interface IFoldersRepository
{
    public System.Threading.Tasks.Task CreateFolderAsync(Folder folder);
}
