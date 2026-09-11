using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniInstagramASP.Models;
using MiniInstagramASP.Services;
using MiniInstagramEF.context;
using MiniInstagramEF.entities;

namespace MiniInstagramASP.Controllers;


[Route("feed")]
public class FeedController : Controller
{
    private readonly MiniInstagramContext _context;
    private readonly CurrentUserService _currentUserService;
    
    public FeedController(MiniInstagramContext context, CurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    [HttpGet("")]
    [HttpGet("load")]
    public async Task<IActionResult> Load()
    {
        _currentUserService.ReloadUserAsync();
        var authUser = _currentUserService.IdentityUser;
        MainPageModel mainPageModel;
        
        if (authUser == null)
        {
            var posts = await ConstructFeedNotLogged();
            
            mainPageModel = new MainPageModel(null, posts);
        }
        else
        {
            var domainUser = await _context.Users
                .Include(u => u.Posts)
                .Include(u => u.Comments)
                .Include(u => u.Followers)
                .Include(u => u.Following)
                .Include(u => u.LikedPosts)
                .Include(u => u.LikedComments)
                .FirstOrDefaultAsync(u => u.Id == authUser.DomainUserId);

            
            if (domainUser == null)
            {
                return Unauthorized();
            }
            
            var posts = await ConstructFeed(domainUser);
            
            mainPageModel = new MainPageModel(domainUser, posts);
        }
        
        return View("Feed", mainPageModel);
        
    }

    public async Task<List<Post>> ConstructFeedNotLogged()
    {
        var feed = await _context.Posts
            .Include(p => p.Author)
            .Include(p => p.Comments)
            .Include(p => p.LikedBy)
            .OrderByDescending(p => p.ReleaseDate)
            .ToListAsync();
        
        return feed;
    }
    
    public async Task<List<Post>> ConstructFeed(User user)
    {
        var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);
        var userId = user.Id;

        var followedUserIds = await _context.Users
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Following.Select(f => f.Id))
            .ToListAsync();

        var likedAuthorIds = await _context.Posts
            .Where(p => p.LikedBy.Any(u => u.Id == userId))
            .Select(p => p.AuthorId)
            .Where(authorId => !followedUserIds.Contains(authorId))
            .Distinct()
            .ToListAsync();

        var freshFollowedPostsQuery = _context.Posts
            .Where(p => followedUserIds.Contains(p.AuthorId) &&
                        p.ReleaseDate >= sevenDaysAgo);

        var randomLikedPostsQuery = _context.Posts
            .Where(p => likedAuthorIds.Contains(p.AuthorId) &&
                        p.ReleaseDate >= sevenDaysAgo)
            .OrderBy(p => p.Id)
            .Take(50);

        var feed = await freshFollowedPostsQuery
            .Union(randomLikedPostsQuery)
            .Where(p => p.AuthorId != userId) 
            .Include(p => p.Author)
            .Include(p => p.Comments)
            .Include(p => p.LikedBy)
            .OrderByDescending(p => p.ReleaseDate)
            .ThenBy(p => p.Id)
            .ToListAsync();

        return feed;
    }


}
