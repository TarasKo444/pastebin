using System.Security.Claims;
using Pastebin.Api.Exceptions;

namespace Pastebin.Api.Services;

public class UserContext(IHttpContextAccessor contextAccessor)
{
    private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

    public ClaimsPrincipal User => _contextAccessor.HttpContext?.User ??
                                      throw new ApiException(500, "HttpContext is null");
}