using System.ComponentModel.DataAnnotations;

namespace MiniInstagramEF.entities;

public class Message
{
    public int Id { get; set; }
    
    public int SenderId { get; set; }
    
    public virtual User Sender { get; set; }
    
    public DateTime SentAt { get; set; }
    
    public int ReceiverId { get; set; }

    public virtual User Receiver { get; set; }

    [Required]
    public string Text { get; set; }
    
}