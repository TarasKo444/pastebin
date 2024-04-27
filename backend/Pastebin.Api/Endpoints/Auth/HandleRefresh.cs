using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Pastebin.Api.Extensions;
using Pastebin.Api.Services;
using Pastebin.Application.Commands;
using Pastebin.Common.Options;

namespace Pastebin.Api.Endpoints.Auth;

public class HandleRefresh : IModule
{
    public static async Task<IResult> Handle(
        HttpContext httpContext,
        [FromServices] ISender sender,
        [FromServices] IOptions<JwtOptions> jwtOptions,
        [FromServices] CookieService cookieService)
    {
        var accessToken = httpContext.Request.Headers[HeaderNames.Authorization]
            .ToString()
            .Replace("Bearer ", string.Empty);

        if (accessToken is null)
        {
            return CustomResults.ErrorJson(400, [Error.Failure(description: "access token not found in headers")]);
        }

        var refreshToken = httpContext.Request.Cookies["refresh-token"];

        if (refreshToken is null)
        {
            return CustomResults.ErrorJson(401, [Error.Failure(description: "refresh token not found, reauthorize")]);
        }

        var response = await sender.Send(new RefreshTokenRequest
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        });

        if (!response.IsError)
        {
            cookieService.RegisterRefreshTokenCookie(httpContext, response.Value.RefreshToken);
            return Results.Json(response.Value.AccessToken);
        }

        var error = response.FirstError;
        return CustomResults.ErrorJson(error.Type, [error]);
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("auth/refresh", Handle);
        return endpoints;
    }
}