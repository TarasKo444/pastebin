using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Pastebin.Common;
using Pastebin.Common.Options;
using Pastebin.Infrastructure;
using Pastebin.Infrastructure.Services;

namespace Pastebin.Application.Commands.Paste;

public record CreatePasteRequest : IRequest<ErrorOr<CreatePasteResponse>>
{
    public required string Text { get; set; }
    public required string Title { get; set; }
    public Guid? CreatorId { get; set; }
}

public record CreatePasteResponse
{
    public required string Id { get; set; }
    public required string Text { get; set; }
    public required string Title { get; set; }
    public Guid? CreatorId { get; set; }
}

public class CreatePasteRequestHandler(
    AppDbContext appDbContext,
    IOptions<JwtOptions> jwtOptions)
    : IRequestHandler<CreatePasteRequest, ErrorOr<CreatePasteResponse>>
{
    private readonly AppDbContext _appDbContext = appDbContext;
    private readonly IOptions<JwtOptions> _jwtOptions = jwtOptions;

    public async Task<ErrorOr<CreatePasteResponse>> Handle(CreatePasteRequest request,
        CancellationToken cancellationToken)
    {
        if (request.CreatorId is not null)
        {
            if (!await _appDbContext.Users.AnyAsync(u => u.Id == request.CreatorId,
                    cancellationToken: cancellationToken))
            {
                return Error.Failure(description: "User does not exist");
            }
        }

        var paste = new Domain.Entities.Paste()
        {
            Text = request.Text,
            Title = request.Title,
            CreatorId = request.CreatorId,
            Id = Hasher.GenerateAlphabeticString(8)
        };

        await _appDbContext.Pastes.AddAsync(paste, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);

        return paste.Adapt<CreatePasteResponse>();
    }
}