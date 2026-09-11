namespace MiniInstagramASP.Models;

public class CommentViewModel
{
    public int AuthorId { get; set; }
    public int PostId { get; set; }
    public string Text { get; set; }
}