using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pastebin.Api.Extensions;
using Pastebin.Api.Services;
using Pastebin.Application.Commands.Login;
using Pastebin.Common.Options;
using Pastebin.Infrastructure.Services;

namespace Pastebin.Api.Endpoints.Auth;

public class HandleCallback : IModule
{
    public static async Task<IResult> HandleAsync(
        [FromServices] ExternalAuthService externalAuthService,
        [FromServices] IOptions<JwtOptions> jwtOptions,
        [FromServices] CookieService cookieService,
        [FromServices] JwtService jwtService,
        [FromServices] ISender sender,
        HttpContext httpContext,
        [FromQuery] string code)
    {
        var callback = $"https://{httpContext.Request.Host}{httpContext.Request.Path}";
        var tokens = await externalAuthService.GetCredentials(code, callback);

        if (tokens.IsError)
        {
            return CustomResults.ErrorJson(400, tokens.Errors);
        }

        var userInfo = await externalAuthService.GetUserInfo(tokens.Value.AccessToken);
        
        var (userResponse, refreshToken, accessToken) = await sender.Send(userInfo.Adapt<GoogleLoginUserRequest>());
        
        if (userResponse.IsError)
        {
            var type = userResponse.FirstError.Type;
            return CustomResults.ErrorJson(type, userResponse.Errors);
        }
        
        cookieService.RegisterRefreshTokenCookie(httpContext, refreshToken);
        
        return Results.Ok(new
        {
            access_token = accessToken,
            user_info = userResponse.Value
        });
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/auth/google/callback", HandleAsync);
        return endpoints;
    }
}