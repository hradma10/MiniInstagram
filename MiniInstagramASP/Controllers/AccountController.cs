using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniInstagramASP.Models;
using MiniInstagramASP.Services;
using MiniInstagramEF.context;

namespace MiniInstagramASP.Controllers;

[Route("account")]
public class AccountController : Controller
{
    private readonly MiniInstagramContext _context;
    private readonly CurrentUserService _currentUserService;

    public AccountController(MiniInstagramContext context, CurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    
    [HttpGet]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Account(int? id)
    {
        _currentUserService.ReloadUserAsync();
        
        int searchedId;
        var authUser = _currentUserService.IdentityUser;
        
        if (authUser == null)
            return Unauthorized();
        bool ownAccount;
        if (id == null)
        {
            searchedId = authUser.DomainUserId ?? throw new ArgumentNullException(nameof(authUser.DomainUserId));
            ownAccount = true;
        }
        else
        {
            searchedId = id ?? throw new ArgumentNullException(nameof(id));
            ownAccount = authUser.DomainUserId == id;
        }

        var domainUser = await _context.Users
            .Include(u => u.Posts)
            .Include(u => u.Comments)
            .Include(u => u.Followers)
            .Include(u => u.Following)
            .Include(u => u.LikedPosts)
            .Include(u => u.LikedComments)
            .FirstOrDefaultAsync(u => u.Id == searchedId);

        if (domainUser == null)
        {
            return NotFound();
        }
        
        var model = new AccountViewModel(domainUser, ownAccount, authUser.DomainUserId);
        
        return View("Account", model);

    }
    
    
    
    [HttpGet("edit")]
    public IActionResult EditAccountDetails()
    {
        _currentUserService.ReloadUserAsync();
        
        var user = _currentUserService.EfUser;
        
        if (user == null)
        {
            return NotFound();
        }
        
        var model = new AccountEditModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Age = user.Age,
            Username =  user.Username,
            Description = user.Description ?? ""
        };
        
        return View("EditAccount", model);

    }
    
    [HttpPost("edit")]
    public async Task<IActionResult> EditAccountDetailsPost(AccountEditModel model)
    {
        _currentUserService.ReloadUserAsync();
        
        var user = _currentUserService.EfUser;
        
        if (user == null)
        {
            return NotFound();
        }
        
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Age = model.Age;
        user.Username = model.Username;
        user.Description = model.Description;
        
        await _context.SaveChangesAsync();
        
        return RedirectToAction("Account", "Account");

    }
    
    [HttpPost("follow/{id:int}")]
    public async Task<IActionResult> ToggleFollow(int id, bool account = false)
    {
        _currentUserService.ReloadUserAsync();
        
        var user = _currentUserService.IdentityUser;
        
        if (user == null)
            return Unauthorized();
        
        var currentUser = await _context.Users.Include(u => u.Following).Include(u => u.Followers).FirstOrDefaultAsync(u => u.Id == user.DomainUserId);
        
        if (currentUser == null)
            return Unauthorized();
        
        var userToFollow = await _context.Users.Include(u => u.Following).Include(u => u.Followers).FirstOrDefaultAsync(u => u.Id == id);
        if (userToFollow == null)
            return NotFound();

        bool followed;
        if (currentUser.Following.Contains(userToFollow))
        {
            currentUser.Following.Remove(userToFollow);
            followed = false;
        }
        else
        {
            currentUser.Following.Add(userToFollow);
            followed = true;
        }

        await _context.SaveChangesAsync();

        var result = new 
        {
            buttonText = followed ? "Unfollow" : "Follow",
            followers = account ? userToFollow.Followers.Count : (int?)null,
            following = account ? userToFollow.Following.Count : (int?)null
        };

        return new JsonResult(result);
        
    }
    
}