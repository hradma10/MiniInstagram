using System.ComponentModel.DataAnnotations;

namespace MiniInstagramEF.entities;

public class Comment
{
    public int Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    [Required]
    public string Text { get; set; }
    
    public int AuthorId { get; set; }
    
    public virtual User Author { get; set; }
    
    public int PostId { get; set; }
    
    public virtual Post Post { get; set; }
    
    public virtual ICollection<User> LikedBy { get; set; } = new List<User>();
    
}