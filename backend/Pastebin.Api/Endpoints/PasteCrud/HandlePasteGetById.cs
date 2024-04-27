using MediatR;
using Pastebin.Api.Extensions;
using Pastebin.Application.Commands.Paste;

namespace Pastebin.Api.Endpoints.PasteCrud;

public class HandlePasteGetById : IModule
{
    public static async Task<IResult> Handle(string id, ISender sender)
    {
        var result = await sender.Send(new GetPasteByIdRequest { Id = id });

        if (!result.IsError) return Results.Json(result.Value);

        var error = result.FirstError;
        return CustomResults.ErrorJson(error.Type, [error]);
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/paste/{*id}", Handle);
        return endpoints;
    }
}