using AutoMapper;
using MediatR;
using TimeToDo.Application.Dtos;
using TimeToDo.Application.Interfaces.Infrastructure.Repositories;
using TimeToDo.Shared;

namespace TimeToDo.Application.Queries;
public class GetFoldersQuery : IRequest<List<FolderDto>>
{
}

public class GetFoldersQueryHandler : IRequestHandler<GetFoldersQuery, List<FolderDto>>
{
    private readonly IFoldersRepository _foldersRepository;
    private readonly IRequestUser _requestUser;
    private readonly IMapper _mapper;

    public GetFoldersQueryHandler(IFoldersRepository foldersRepository, IRequestUser requestUser, IMapper mapper)
    {
        _foldersRepository = foldersRepository;
        _requestUser = requestUser;
        _mapper = mapper;
    }

    public async Task<List<FolderDto>> Handle(GetFoldersQuery request, CancellationToken cancellationToken)
    {
        var folders =  await _foldersRepository.GetlFoldersAsync(_requestUser.Id);

        return _mapper.Map<List<FolderDto>>(folders);
    }
}
