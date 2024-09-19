using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace TimeToDo.Shared.Extensions;
public static class HttpContextExtensions
{
    public static Guid GetUserId(this HttpContext httpContext)
    {
        var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        var userId = new Guid(userIdClaim!.Value);
        return userId;
    }
}
