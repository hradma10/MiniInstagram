using System.ComponentModel.DataAnnotations;

namespace MiniInstagramEF.entities;

public class Post
{
    public int Id { get; set; }

    [Required]
    public string Text { get; set; }
    
    [Required]
    public string ImgPath { get; set; }

    public string ImgHash { get; set; }
    
    public virtual ICollection<User> Tags { get; set; } = new List<User>();
    
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    
    public virtual ICollection<User> LikedBy { get; set; } = new List<User>();
    
    public int AuthorId { get; set; }
    
    public virtual User Author { get; set; }
    
    public DateTime ReleaseDate { get; set; }
}