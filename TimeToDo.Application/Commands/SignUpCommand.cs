using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TimeToDo.Application.Common;
using TimeToDo.Domain.Entities;
using TimeToDo.Domain.Models;

namespace TimeToDo.Application.Commands;
public class SignUpCommand : IRequest<AuthResult>
{
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class SignUpCommandHandler : AuthBaseHandler, IRequestHandler<SignUpCommand, AuthResult>
{
    public SignUpCommandHandler(UserManager<User> userManager, IConfiguration configuration) : base(configuration, userManager)
    {

    }

    public async Task<AuthResult> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        var user = new User()
        {
            Email = request.Email,
            UserName = request.Email.Split('@').ElementAtOrDefault(0)
        };

        var createdUser = await _userManager.CreateAsync(user, password: request.Password);

        if (!createdUser.Succeeded)
        {
            return new AuthResult
            {
                Errors = createdUser.Errors.Select(x => x.Description),
                Success = false
            };
        }

        return new AuthResult
        {
            Success = true,
            Token = CreateJwtToken(user),
        };
    }
}
