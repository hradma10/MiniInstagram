using MiniInstagramEF.entities;

namespace MiniInstagramASP.Models;

public class AccountViewModel
{
    public User User { get; set; }
    public bool OwnAccount { get; set; }
    
    public int? LoggedUserId { get; set; }
    
    public AccountViewModel(User user, bool ownAccount, int? loggedUserId)
    {
        this.User = user;
        this.OwnAccount = ownAccount;
        this.LoggedUserId = loggedUserId;
    }
}