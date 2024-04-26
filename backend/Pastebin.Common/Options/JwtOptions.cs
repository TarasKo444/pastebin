namespace Pastebin.Common.Options;

public class JwtOptions
{
    public string Key { get; set; } = null!;
    public string HashKey { get; set; } = null!;
    public int LifetimeInMinutes { get; set; }
    public int RefreshTokenExpiryTimeInDays { get; set; }

    public bool ValidateLifetime { get; set; }
    public bool ValidateAudience { get; set; }
    public bool ValidateIssuer { get; set; }
    public string[] ValidAudiences { get; set; } = [];
    public string[] ValidIssuers { get; set; } = [];
}