using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
    public required Guid? CreatorId { get; set; }
}

public class GetPasteByIdRequestHandler(AppDbContext appDbContext)
    : IRequestHandler<GetPasteByIdRequest, ErrorOr<GetPasteByIdResponse>>
{
    private readonly AppDbContext _appDbContext = appDbContext;

    public async Task<ErrorOr<GetPasteByIdResponse>> Handle(GetPasteByIdRequest request,
        CancellationToken cancellationToken)
    {
        var paste = await _appDbContext.Pastes.FirstOrDefaultAsync(p => p.Id == request.Id,
            cancellationToken: cancellationToken);

        if (paste is null)
            return Error.Failure(description: "Paste with given id not found");

        return paste.Adapt<GetPasteByIdResponse>();
    }
}