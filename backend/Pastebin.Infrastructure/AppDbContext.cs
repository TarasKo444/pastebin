using Microsoft.EntityFrameworkCore;
using Pastebin.Domain.Entities;

namespace Pastebin.Infrastructure;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Paste> Pastes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(user =>
        {
            user.HasKey(u => u.Id);
            user.Property(u => u.Sub).IsRequired(false);
            user.Property(u => u.IsGoogleUser).HasDefaultValue(false);
            user.Property(u => u.Picture).IsRequired(false);
            user.Property(u => u.PasswordHash).IsRequired(false);
            user.HasIndex(u => u.Email).IsUnique();
        });

        modelBuilder.Entity<Paste>(paste =>
        {
            paste.HasKey(p => p.Id);
            paste.Property(p => p.CreatorId).IsRequired(false);
            paste.Property(p => p.Text).IsRequired();
            paste.Property(p => p.Title).IsRequired().HasDefaultValue("Untitled");
            paste.Property(p => p.ExpirationTime).IsRequired(false);
            paste.HasOne(p => p.Creator)
                .WithMany(u => u.Pastes)
                .HasForeignKey(p => p.CreatorId);
        });
    }
}