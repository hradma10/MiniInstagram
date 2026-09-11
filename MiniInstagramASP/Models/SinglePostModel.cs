using MiniInstagramEF.entities;

namespace MiniInstagramASP.Models;

public class SinglePostModel
{
    public SinglePostModel(Post post, User? currentUser)
    {
        Post = post;
        User = currentUser;
    }
    
    public Post Post { get; set; }
    public User? User { get; set; }
}