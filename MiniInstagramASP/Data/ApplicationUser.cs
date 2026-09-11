using Microsoft.AspNetCore.Identity;
using MiniInstagramEF.entities;

namespace MiniInstagramASP.Data;

public class ApplicationUser : IdentityUser
{
    public int? DomainUserId { get; set; }
}
