namespace Pastebin.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string? Sub { get; set; }
    public string Username { get; set; } = null!;
    public string? Picture { get; set; }
    public string? PasswordHash { get; set; }
    public bool IsGoogleUser { get; set; }
    public string Email { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public DateTime RefreshTokenExpiryTime { get; set; }

    public ICollection<Paste> Pastes { get; set; } = [];
}