using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.Extensions.Options;
using Pastebin.Common;
using Pastebin.Common.Options;
using Pastebin.Domain.Entities;
using Pastebin.Infrastructure;
using Pastebin.Infrastructure.Services;

namespace Pastebin.Application.Commands.Login;

public record
    GoogleLoginUserRequest : IRequest<(ErrorOr<GoogleLoginUserResponse>, string refreshToken, string accessToken)>
{
    public required string Sub { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Picture { get; set; }
}

public record GoogleLoginUserResponse
{
    public required Guid Id { get; set; }
    public required string Sub { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Picture { get; set; }
}

public class GoogleLoginUserRequestHandler(
    AppDbContext appDbContext,
    IOptions<JwtOptions> jwtOptions,
    JwtService jwtService)
    : IRequestHandler<GoogleLoginUserRequest, (ErrorOr<GoogleLoginUserResponse>, string refreshToken, string accessToken)>
{
    private readonly AppDbContext _appDbContext = appDbContext;
    private readonly IOptions<JwtOptions> _jwtOptions = jwtOptions;
    private readonly JwtService _jwtService = jwtService;

    public async Task<(ErrorOr<GoogleLoginUserResponse>, string refreshToken, string accessToken)> Handle(
        GoogleLoginUserRequest request, CancellationToken cancellationToken)
    {
        var nonGoogleUserExists = _appDbContext.Users.FirstOrDefault(u => u.Email == request.Email && !u.IsGoogleUser);

        if (nonGoogleUserExists is not null)
            return (Error.Failure(description: "user with given email already exists"), "", "");

        var user = _appDbContext.Users.FirstOrDefault(u => u.Sub == request.Sub && u.IsGoogleUser);

        if (user is null)
        {
            user = request.Adapt<User>();
            user.Id = Guid.NewGuid();
            await _appDbContext.Users.AddAsync(user, cancellationToken);
        }
        else
        {
            user.Username = request.Username;
            user.Email = request.Email;
            user.Picture = request.Picture;
        }

        var refreshToken = Hasher.GenerateHash(64);

        user.RefreshToken = _jwtService.Hash(refreshToken);
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtOptions.Value.RefreshTokenExpiryTimeInDays);
        user.IsGoogleUser = true;

        await _appDbContext.SaveChangesAsync(cancellationToken);
        return (user.Adapt<GoogleLoginUserResponse>(), refreshToken, _jwtService.GenerateToken(user));
    }
}