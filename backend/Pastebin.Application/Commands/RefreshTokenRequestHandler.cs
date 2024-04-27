using ErrorOr;
using MediatR;
using Microsoft.Extensions.Options;
using Pastebin.Common;
using Pastebin.Common.Options;
using Pastebin.Infrastructure;
using Pastebin.Infrastructure.Services;

namespace Pastebin.Application.Commands;

public class RefreshTokenRequest : IRequest<ErrorOr<RefreshTokenResponse>>
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}

public class RefreshTokenResponse
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}

public class RefreshTokenRequestHandler(
    JwtService jwtService,
    AppDbContext appDbContext,
    IOptions<JwtOptions> jwtOptions)
    : IRequestHandler<RefreshTokenRequest, ErrorOr<RefreshTokenResponse>>
{
    private readonly JwtService _jwtService = jwtService;
    private readonly IOptions<JwtOptions> _jwtOptions = jwtOptions;
    private readonly AppDbContext _appDbContext = appDbContext;

    public async Task<ErrorOr<RefreshTokenResponse>> Handle(RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        var userInfo = _jwtService.GetPrincipalFromExpiredToken(request.AccessToken);

        if (userInfo.IsError)
        {
            return userInfo.FirstError;
        }

        var id = userInfo.Value.Claims.FirstOrDefault(c => c.Type == "id");

        if (id is null)
        {
            return Error.Failure(description: "wrong user info");
        }

        var user = _appDbContext.Users.FirstOrDefault(u => u.Id == Guid.Parse(id.Value));

        if (user is null)
        {
            return Error.NotFound(description: "user not found");
        }

        if (user.RefreshToken != _jwtService.Hash(request.RefreshToken))
        {
            return Error.Unauthorized(description: "refresh token is wrong");
        }

        if (DateTime.UtcNow > user.RefreshTokenExpiryTime)
        {
            return Error.Unauthorized(description: "refresh token expired");
        }

        var response = new RefreshTokenResponse
        {
            AccessToken = _jwtService.GenerateToken(user),
            RefreshToken = Hasher.GenerateHash(64)
        };

        user.RefreshToken = _jwtService.Hash(response.RefreshToken);
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtOptions.Value.RefreshTokenExpiryTimeInDays);

        await _appDbContext.SaveChangesAsync(cancellationToken);

        return response;
    }
}