namespace Pastebin.Domain.Entities;

public class Paste
{
    public string Id { get; set; } = null!;
    public string Text { get; set; } = null!;
    public string Title { get; set; } = null!;
    public DateTimeOffset? ExpirationTime { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    
    public Guid? CreatorId { get; set; }
    public User? Creator { get; set; }
}
