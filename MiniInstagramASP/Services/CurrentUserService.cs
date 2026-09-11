using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniInstagramASP.Data;
using MiniInstagramEF.context;
using MiniInstagramEF.entities;

namespace MiniInstagramASP.Services;

public class CurrentUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly MiniInstagramContext _context;
    
    public User? EfUser { get; private set; }
    
    public ApplicationUser? IdentityUser { get; private set; }
    
    public CurrentUserService(
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor,
        MiniInstagramContext context)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }
    public async void ReloadUserAsync()
    {
        try
        {
            var principal = _httpContextAccessor.HttpContext?.User;
            if (principal == null)
            {
                IdentityUser = null;
                EfUser = null;
                return;
            }

            IdentityUser = await _userManager.GetUserAsync(principal);

            if (IdentityUser == null)
            {
                EfUser = null;
                return;
            }
        
            EfUser = await _context.Users
                .FirstOrDefaultAsync(u => u.ApplicationUserId == IdentityUser.Id);
        }
        catch (Exception e)
        {
            return; 
        }
    }
    
    
}