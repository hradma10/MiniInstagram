using Microsoft.EntityFrameworkCore;
using MiniInstagramEF.entities;

namespace MiniInstagramEF.context;

public class MiniInstagramContext : DbContext
{
    public MiniInstagramContext(DbContextOptions<MiniInstagramContext> options)
        : base(options) 
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Post> Posts { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
        
        modelBuilder.Entity<Post>()
            .HasMany(p => p.Tags)
            .WithMany(u => u.TaggedInPosts)
            .UsingEntity(j => j.ToTable("TaggedUsers"));

        modelBuilder.Entity<User>()
            .HasMany(u => u.Following)
            .WithMany(u => u.Followers)
            .UsingEntity(j => j.ToTable("UserFollows"));
        
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany(u => u.SentMessages)
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Receiver)
            .WithMany(u => u.ReceivedMessages)
            .HasForeignKey(m => m.ReceiverId)
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<Comment>()
            .HasMany(c => c.LikedBy)
            .WithMany(u => u.LikedComments)
            .UsingEntity(j => j.ToTable("CommentLikes"));
        
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Author)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Post)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Post>()
            .HasOne(p => p.Author)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Post>()
            .HasMany(p => p.LikedBy)
            .WithMany(u => u.LikedPosts)
            .UsingEntity(j => j.ToTable("PostLikes"));
    }
}