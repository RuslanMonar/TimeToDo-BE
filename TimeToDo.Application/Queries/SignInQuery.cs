using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TimeToDo.Application.Common;
using TimeToDo.Domain.Entities;
using TimeToDo.Domain.Models;

namespace TimeToDo.Application.Queries;
public class SignInQuery : IRequest<AuthResult>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class SignInQueryHandler : AuthBaseHandler, IRequestHandler<SignInQuery, AuthResult>
{
    private readonly SignInManager<User> _signInManager;

    public SignInQueryHandler(UserManager<User> userManager, IConfiguration configuration,
        SignInManager<User> signInManager) : base(configuration, userManager)
    {
        _signInManager = signInManager;
    }

    public async Task<AuthResult> Handle(SignInQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        var result = await _signInManager
            .CheckPasswordSignInAsync(user, request.Password, false);

        if (result.Succeeded)
        {
            return new AuthResult
            {
                Success = true,
                Token = CreateJwtToken(user)
            };
        }

        return new AuthResult()
        {
            Errors = new List<string> { "Bad Email or Password" }
        };
    }
}
