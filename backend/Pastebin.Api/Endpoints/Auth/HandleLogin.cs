using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pastebin.Api.Exceptions;
using Pastebin.Api.Extensions;
using Pastebin.Api.Services;
using Pastebin.Application.Commands.Login;

namespace Pastebin.Api.Endpoints.Auth;

public class HandleLogin : IModule
{
    public class LoginRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }

        public class Validator : AbstractValidator<LoginRequest>
        {
            public Validator()
            {
                RuleFor(b => b.Email).NotEmpty();
                RuleFor(b => b.Password).NotEmpty();
            }
        }
    }

    public static async Task<IResult> Handle(
        HttpContext httpContext,
        [FromBody] LoginRequest? request,
        [FromServices] IValidator<LoginRequest> validator,
        [FromServices] ISender sender,
        [FromServices] CookieService cookieService)
    {
        await ApiException.ValidateAsync(validator, request);
        
        var (response, refreshToken, accessToken) = await sender.Send(request.Adapt<LoginUserRequest>());
        
        if (response.IsError)
        {
            var type = response.FirstError.Type;
            return CustomResults.ErrorJson(type, response.Errors);
        }
        
        cookieService.RegisterRefreshTokenCookie(httpContext, refreshToken);
        
        return Results.Ok(new
        {
            access_token = accessToken,
            user_info = response.Value
        });
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("auth/login", Handle);
        return endpoints;
    }
}