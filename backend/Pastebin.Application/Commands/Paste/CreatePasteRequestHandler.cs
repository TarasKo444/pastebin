using System.Linq.Expressions;
using ErrorOr;
using Hangfire;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Pastebin.Common;
using Pastebin.Common.Options;
using Pastebin.Infrastructure;

namespace Pastebin.Application.Commands.Paste;

public record CreatePasteRequest : IRequest<ErrorOr<CreatePasteResponse>>
{
    public required string Text { get; set; }
    public required string? Title { get; set; }
    public DateTimeOffset? ExpirationTime { get; set; }
    public Guid? CreatorId { get; set; }
}

public record CreatePasteResponse
{
    public required string Id { get; set; }
    public required string Text { get; set; }
    public required string Title { get; set; }
    public DateTimeOffset? ExpirationTime { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public Guid? CreatorId { get; set; }
}

public class CreatePasteRequestHandler(
    AppDbContext appDbContext,
    IOptions<JwtOptions> jwtOptions,
    IDistributedCache cache)
    : IRequestHandler<CreatePasteRequest, ErrorOr<CreatePasteResponse>>
{
    private readonly AppDbContext _appDbContext = appDbContext;
    private readonly IOptions<JwtOptions> _jwtOptions = jwtOptions;
    private readonly IDistributedCache _cache = cache;

    public async Task<ErrorOr<CreatePasteResponse>> Handle(CreatePasteRequest request,
        CancellationToken cancellationToken)
    {
        if (request.ExpirationTime is not null && request.ExpirationTime < DateTimeOffset.UtcNow)
        {
            return Error.Failure(description: "Requested time already expired");
        }
        
        if (request.CreatorId is not null)
        {
            if (!await _appDbContext.Users.AnyAsync(u => u.Id == request.CreatorId,
                    cancellationToken: cancellationToken))
            {
                return Error.Failure(description: "User does not exist");
            }
        }

        var paste = request.Adapt<Domain.Entities.Paste>();
        paste.Id = Hasher.GenerateAlphabeticString(8);
        paste.CreatedAt = DateTimeOffset.UtcNow;
        
        await _appDbContext.Pastes.AddAsync(paste, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);
        
        if (request.ExpirationTime is not null)
        {
            BackgroundJob.Schedule(() => DeletePaste(paste.Id), request.ExpirationTime.Value);
        }
        
        return paste.Adapt<CreatePasteResponse>();
    }

    public async Task DeletePaste(string id)
    {
        await _cache.RemoveAsync(id);
        var paste = await _appDbContext.Pastes.FirstOrDefaultAsync(p => p.Id == id);

        if (paste is not null)
        {
            _appDbContext.Pastes.Remove(paste);
            await _appDbContext.SaveChangesAsync();
        }
    }
}