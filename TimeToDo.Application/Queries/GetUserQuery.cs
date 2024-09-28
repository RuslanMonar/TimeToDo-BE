using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TimeToDo.Application.Dtos;
using TimeToDo.Domain.Entities;
using TimeToDo.Shared;
using TimeToDo.Shared.Exceptions;

namespace TimeToDo.Application.Queries;
public class GetUserQuery : IRequest<UserDto>
{
}

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
{
    private readonly IRequestUser _requestUser;
    protected readonly UserManager<User> _userManager;
    protected readonly IMapper _mapper;

    public GetUserQueryHandler(IRequestUser requestUser, UserManager<User> userManager, IMapper mapper)
    {
        _requestUser = requestUser;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(_requestUser.Id.ToString());

        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        return _mapper.Map<UserDto>(user);
    }
}
