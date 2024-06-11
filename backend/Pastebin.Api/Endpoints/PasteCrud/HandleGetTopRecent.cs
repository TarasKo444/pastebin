using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pastebin.Api.Extensions;
using Pastebin.Application.Commands.Paste;

namespace Pastebin.Api.Endpoints.PasteCrud;

public class HandleGetTopRecent : IModule
{
    public static async Task<IResult> Handle(
        [FromServices] ISender sender,
        [FromQuery] int count = 1)
    {
        var result = await sender.Send(new GetTopRecentRequest { Count = count });
        
        if (!result.IsError) return Results.Json(result.Value);

        var error = result.FirstError;
        return CustomResults.ErrorJson(error.Type, [error]);
    }
    
    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/paste/recent", Handle);
        return endpoints;
    }
}
