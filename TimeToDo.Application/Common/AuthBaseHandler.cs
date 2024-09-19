using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TimeToDo.Domain.Entities;

namespace TimeToDo.Application.Common;
public class AuthBaseHandler
{
    private readonly IConfiguration _configuration;
    protected readonly UserManager<User> _userManager;

    public AuthBaseHandler(IConfiguration configuration, UserManager<User> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    protected string CreateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JwtTokenKey"]);
        var audience = _configuration["JwtTokenAudience"];

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new (ClaimTypes.Name, user.UserName),
                new (ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = audience
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
