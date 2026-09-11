using System.ComponentModel.DataAnnotations;

namespace MiniInstagramEF.entities;

public class User
{
    public int Id { get; set; }
    
    public string ApplicationUserId { get; set; }
    
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    public int Age { get; set; }
    
    public string? Description { get; set; }
    
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    
    public virtual ICollection<User> Following { get; set; } = new List<User>();
    
    public virtual ICollection<User> Followers { get; set; } = new List<User>();
    
    public virtual ICollection<Message> SentMessages { get; set; } = new List<Message>();
    
    public virtual ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();
    
    public virtual ICollection<Post> TaggedInPosts { get; set; } = new List<Post>();
    
    public virtual ICollection<Post> LikedPosts { get; set; } = new List<Post>();
    public virtual ICollection<Comment> LikedComments { get; set; } = new List<Comment>();

}