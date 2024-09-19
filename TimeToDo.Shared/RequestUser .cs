using Microsoft.AspNetCore.Http;
using TimeToDo.Shared.Extensions;

namespace TimeToDo.Shared;
public class RequestUser : IRequestUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RequestUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid Id
    {
        get
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext == null)
            {
                throw new InvalidOperationException("HttpContext is not available.");
            }

            return httpContext.GetUserId();
        }
    }
}
