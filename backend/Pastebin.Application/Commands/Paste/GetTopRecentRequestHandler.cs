using System.Text.Json;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Pastebin.Infrastructure;

namespace Pastebin.Application.Commands.Paste;

public record GetTopRecentRequest : IRequest<ErrorOr<List<GetTopRecentResponse>>>
{
    public int Count { get; set; }
}

public record GetTopRecentResponse
{
    public required string Id { get; set; }
    public required string Text { get; set; }
    public required string Title { get; set; }
    public required DateTimeOffset? ExpirationTime { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public required Guid? CreatorId { get; set; }
}

public class GetTopRecentRequestHandler(AppDbContext appDbContext, IDistributedCache cache) : IRequestHandler<GetTopRecentRequest, ErrorOr<List<GetTopRecentResponse>>>
{
    private readonly AppDbContext _appDbContext = appDbContext;
    private readonly IDistributedCache _cache = cache;
    private const string cacheName = "topRecent";

    public async Task<ErrorOr<List<GetTopRecentResponse>>> Handle(GetTopRecentRequest request,
        CancellationToken cancellationToken)
    {
        if (request.Count < 1)
        {
            return Error.Failure(description: "Count is less than 1");
        }
        
        var exists = await _cache.GetStringAsync(cacheName, cancellationToken);
        if (exists is not null)
        {
            return JsonSerializer.Deserialize<List<GetTopRecentResponse>>(exists)!;
        }
        
        var items = _appDbContext.Pastes.OrderByDescending(p => p.CreatedAt).Take(request.Count).Adapt<List<GetTopRecentResponse>>();
        
        await _cache.SetStringAsync(cacheName, JsonSerializer.Serialize(value: items), new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
        }, cancellationToken);

        return items;
    }
}