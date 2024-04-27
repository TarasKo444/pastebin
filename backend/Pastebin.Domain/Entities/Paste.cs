namespace Pastebin.Domain.Entities;

public class Paste
{
    public string Id { get; set; } = null!;
    public string Text { get; set; } = null!;
    public string Title { get; set; } = null!;
    
    public Guid? CreatorId { get; set; }
    public User? Creator { get; set; }
}
