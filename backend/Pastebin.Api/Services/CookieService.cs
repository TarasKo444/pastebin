using Microsoft.Extensions.Options;
using Pastebin.Common.Options;

namespace Pastebin.Api.Services;

public class CookieService(IOptions<JwtOptions> jwtOptions)
{
    private readonly IOptions<JwtOptions> _jwtOptions = jwtOptions;

    public void RegisterRefreshTokenCookie(HttpContext httpContext, string token)
    {
        httpContext.Response.Cookies.Append("refresh-token", token, new()
        {
            Expires = DateTime.UtcNow.AddDays(_jwtOptions.Value.RefreshTokenExpiryTimeInDays),
            Domain = httpContext.Request.Host.Host,
            Path = "/",
            Secure = true
        });
    }
}
