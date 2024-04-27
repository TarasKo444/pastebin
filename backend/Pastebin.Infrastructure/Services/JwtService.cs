using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ErrorOr;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pastebin.Common.Options;
using Pastebin.Domain.Entities;

namespace Pastebin.Infrastructure.Services;

public class JwtService(IOptions<JwtOptions> jwtOptions)
{
    private readonly IOptions<JwtOptions> _jwtOptions = jwtOptions;

    public string GenerateToken(User user)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.Key));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new("id", user.Id.ToString()),
            new("sub", user.Sub ?? ""),
            new("email", user.Email),
            new("name", user.Username),
            new("picture", user.Picture ?? ""),
            new("isGoogleUser", user.IsGoogleUser ? "1" : "0")
        };

        var tokenOptions = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.Value.LifetimeInMinutes),
            signingCredentials: signinCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    public string Hash(string str) =>
        Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(str + _jwtOptions.Value.HashKey)));

    public ErrorOr<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.Key)),
            ValidAudiences = _jwtOptions.Value.ValidAudiences,
            ValidIssuers = _jwtOptions.Value.ValidIssuers,
            ValidateLifetime = false,
            ValidateAudience = _jwtOptions.Value.ValidateAudience,
            ValidateIssuer = _jwtOptions.Value.ValidateIssuer
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            return Error.Failure("Invalid token");
        }

        return principal;
    }
}