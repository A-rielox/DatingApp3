using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<AppUser> Users { get; set; }
    public DbSet<UserLike> Likes { get; set; }
    public DbSet<Message> Messages { get; set; }



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // ------------     USERLIKE
        builder.Entity<UserLike>()
               .HasKey(ul => new { ul.SourceUserId, ul.TargetUserId });

        builder.Entity<UserLike>()
               .HasOne(ul => ul.SourceUser)
               .WithMany(u => u.LikedUsers)
               .HasForeignKey(ul => ul.SourceUserId)
               .OnDelete(DeleteBehavior.Cascade);
               
        builder.Entity<UserLike>()
               .HasOne(ul => ul.TargetUser)
               .WithMany(u => u.LikedByUsers)
               .HasForeignKey(ul => ul.TargetUserId)
               .OnDelete(DeleteBehavior.Cascade);

        // ------------     MESSAGE
        builder.Entity<Message>()
               .HasOne(m => m.Sender)
               .WithMany(u => u.MessagesSent)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Message>()
               .HasOne(m => m.Recipient)
               .WithMany(u => u.MessagesReceived)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
