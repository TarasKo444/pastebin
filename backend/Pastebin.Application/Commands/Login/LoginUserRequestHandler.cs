using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.Extensions.Options;
using Pastebin.Common;
using Pastebin.Common.Options;
using Pastebin.Infrastructure;
using Pastebin.Infrastructure.Services;

namespace Pastebin.Application.Commands.Login;

public record LoginUserRequest : IRequest<(ErrorOr<LoginUserResponse>, string refreshToken, string accessToken)>
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public record LoginUserResponse
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
}

public class LoginUserRequestHandler(
    AppDbContext appDbContext,
    IOptions<JwtOptions> jwtOptions,
    JwtService jwtService)
    : IRequestHandler<LoginUserRequest, (
        ErrorOr<LoginUserResponse>,
        string refreshToken, string accessToken)>
{
    private readonly AppDbContext _appDbContext = appDbContext;
    private readonly IOptions<JwtOptions> _jwtOptions = jwtOptions;
    private readonly JwtService _jwtService = jwtService;

    public async Task<(ErrorOr<LoginUserResponse>, string refreshToken, string accessToken)> Handle(
        LoginUserRequest request,
        CancellationToken cancellationToken)
    {
        var userExists = _appDbContext.Users.FirstOrDefault(u => u.Email == request.Email && !u.IsGoogleUser);

        if (userExists is null || userExists.PasswordHash != _jwtService.Hash(request.Password))
            return (Error.Failure(description: "given email or password is wrong"), "", "");


        var refreshToken = Hasher.GenerateHash(64);

        userExists.RefreshToken = _jwtService.Hash(refreshToken);
        userExists.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtOptions.Value.RefreshTokenExpiryTimeInDays);

        await _appDbContext.SaveChangesAsync(cancellationToken);
        
        return (userExists.Adapt<LoginUserResponse>(), refreshToken, _jwtService.GenerateToken(userExists));
    }
}