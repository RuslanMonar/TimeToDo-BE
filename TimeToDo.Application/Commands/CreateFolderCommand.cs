using MediatR;
using TimeToDo.Application.Interfaces.Infrastructure.Repositories;
using TimeToDo.Domain.Entities;
using TimeToDo.Shared;

namespace TimeToDo.Application.Commands;
public class CreateFolderCommand : IRequest<Unit>
{
    public string Title { get; set; }
    public string Color { get; set; }
}

public class CreateFolderCommandHandler : IRequestHandler<CreateFolderCommand, Unit>
{
    private readonly IFoldersRepository _foldersRepository;
    private readonly IRequestUser _requestUser;

    public CreateFolderCommandHandler(IFoldersRepository foldersRepository, IRequestUser requestUser)
    {
        _foldersRepository = foldersRepository;
        _requestUser = requestUser;
    }

    public async Task<Unit> Handle(CreateFolderCommand request, CancellationToken cancellationToken)
    {
        var folder = new Folder()
        {
            Color = request.Color,
            Title = request.Title,
            UserId = _requestUser.Id
        };

        await _foldersRepository.CreateFolderAsync(folder);

        return Unit.Value;
    }
}
