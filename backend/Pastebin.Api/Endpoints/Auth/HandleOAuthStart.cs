using Microsoft.AspNetCore.Mvc;
using Pastebin.Api.Extensions;
using Pastebin.Infrastructure.Services;

namespace Pastebin.Api.Endpoints.Auth;

public class HandleOAuthStart : IModule
{
    public static IResult Handle(
        HttpContext httpContext,
        [FromServices] ExternalAuthService externalAuthService)
    {
        var callback = $"https://{httpContext.Request.Host}/api/auth/google/callback";
        var uri = externalAuthService.GetGoogleAuthUrl(callback);
        return Results.Redirect(uri);
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/auth/google/login", Handle);
        return endpoints;
    }
}