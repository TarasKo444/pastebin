using System.Globalization;
using System.Security.Claims;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pastebin.Api.Exceptions;
using Pastebin.Api.Extensions;
using Pastebin.Api.JsonConverters;
using Pastebin.Api.Services;
using Pastebin.Application.Commands.Paste;

namespace Pastebin.Api.Endpoints.PasteCrud;

public class HandlePasteCreation : IModule
{
    public static async Task<IResult> Handle(
        [FromServices] IValidator<CreateRequest> validator,
        [FromServices] UserContext userContext,
        [FromServices] ISender sender,
        HttpContext httpContext,
        [FromBody] CreateRequest? request)
    {
        await ApiException.ValidateAsync(validator, request);

        var auth = await httpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);

        Guid? userId = null;

        if (auth.Succeeded)
        {
            var claimsPrincipal = auth.Principal;
            var value = claimsPrincipal.FindFirstValue("id");
            if (value is null)
                throw new ApiException(401, "Not authorized");

            userId = Guid.Parse(value);
        }

        var result = await sender.Send(request.Adapt<CreatePasteRequest>() with
        {
            CreatorId = userId
        });

        if (result.IsError)
        {
            var error = result.FirstError;
            return CustomResults.ErrorJson(error.Type, [error]);
        }
        
        return Results.Json(result.Value);
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("paste", Handle);
        return endpoints;
    }

    public record CreateRequest
    {
        public string? Text { get; set; }
        public string? Title { get; set; }
        [JsonConverter(typeof(DateTimeOffsetJsonConverter))]
        public DateTimeOffset? ExpirationTime { get; set; }

        public class Validator : AbstractValidator<CreateRequest>
        {
            public Validator()
            {
                RuleFor(x => x.Text).NotEmpty();
            }
        }
    }
}