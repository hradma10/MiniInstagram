using MiniInstagramEF.entities;

namespace MiniInstagramASP.Models;

public class MainPageModel
{
    public User? User { get; set; }
    public List<Post> Feed { get; set; }
    
    public MainPageModel(User? user, List<Post> feed)
    {
        this.User = user;
        this.Feed = feed;
    }
}