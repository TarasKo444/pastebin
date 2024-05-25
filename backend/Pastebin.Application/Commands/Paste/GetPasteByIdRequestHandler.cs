using System.Text.Json;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Pastebin.Infrastructure;

namespace Pastebin.Application.Commands.Paste;

public record GetPasteByIdRequest : IRequest<ErrorOr<GetPasteByIdResponse>>
{
    public required string Id { get; set; }
}

public record GetPasteByIdResponse
{
    public required string Id { get; set; }
    public required string Text { get; set; }
    public required string Title { get; set; }
    public required DateTimeOffset? ExpirationTime { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public required Guid? CreatorId { get; set; }
}

public class GetPasteByIdRequestHandler(AppDbContext appDbContext, IDistributedCache cache)
    : IRequestHandler<GetPasteByIdRequest, ErrorOr<GetPasteByIdResponse>>
{
    private readonly AppDbContext _appDbContext = appDbContext;
    private readonly IDistributedCache _cache = cache;

    public async Task<ErrorOr<GetPasteByIdResponse>> Handle(GetPasteByIdRequest request,
        CancellationToken cancellationToken)
    {
        var fromCache = await _cache.GetStringAsync(request.Id, cancellationToken);

        if (fromCache is not null)
        {
            return JsonSerializer.Deserialize<GetPasteByIdResponse>(fromCache)!;
        }

        var paste = await _appDbContext.Pastes.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (paste is null)
            return Error.NotFound(description: "Paste with given id not found");

        var response = paste.Adapt<GetPasteByIdResponse>();
        await _cache.SetStringAsync(response.Id, JsonSerializer.Serialize(response), new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
        }, cancellationToken);

        return response;
    }
}