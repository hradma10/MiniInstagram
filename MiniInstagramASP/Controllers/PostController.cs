using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using MiniInstagramASP.Models;
using MiniInstagramASP.Services;
using MiniInstagramEF.context;
using MiniInstagramEF.entities;

namespace MiniInstagramASP.Controllers;

[Route("posts")]
public class PostController : Controller
{
    private readonly MiniInstagramContext _context;
    private readonly CurrentUserService  _currentUserService;
    private readonly FileManagerService _fileManagerService;
    
    public PostController(MiniInstagramContext context, CurrentUserService currentUserService, FileManagerService fileManagerService)
    {
        _context = context;
        _currentUserService = currentUserService;
        _fileManagerService = fileManagerService;
    }
    
    
    [HttpGet("load/{id}")]
    [HttpGet("render/{id}")]
    public async Task<IActionResult> RenderPost(int id, bool full = false)
    {
        _currentUserService.ReloadUserAsync();
        var authUser = _currentUserService.IdentityUser;
        User? user;
        if (full && authUser != null)
        {
            user = await _context.Users
                .Include(u => u.Posts)
                .Include(u => u.Comments)
                .Include(u => u.Followers)
                .Include(u => u.Following)
                .Include(u => u.LikedPosts)
                .Include(u => u.LikedComments)
                .FirstOrDefaultAsync(u => u.Id == authUser.DomainUserId);
        }
        else
        {
            user = _currentUserService.EfUser;
        }
        
        var post = await _context.Posts
            .Include(p => p.Author).ThenInclude(u => u.Followers)
            .Include(p => p.LikedBy).Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.Id == id);

        ViewBag.CurrentUserId = user?.Id;

        if (post == null)
        {
            return NoContent();
        }

        if (full)
        {
            return View("FullPost", new SinglePostModel(post, currentUser: user));
        };
        
        return PartialView("_Post", post);
    }
    
    [HttpPost("like/{id}")]
    public async Task<IActionResult> ToggleLike(int id)
    {
        _currentUserService.ReloadUserAsync();
        
        var currentUser = _currentUserService.EfUser;
        
        if (currentUser == null)
        {
            return Unauthorized();
        }
        
        var post = _context.Posts.Include(post => post.LikedBy).FirstOrDefaultAsync(p => p.Id == id).Result;

        if (post == null)
        {
            return NotFound();
        }
        
        bool likedPost;
        if (post.LikedBy.Contains(currentUser))
        {
            post.LikedBy.Remove(currentUser);
            likedPost = false;
        }
        else
        {
            post.LikedBy.Add(currentUser);
            likedPost = true;
        }

        await _context.SaveChangesAsync();
        
        return new JsonResult(new
        {
            liked = likedPost,
            likesCount = post.LikedBy.Count
        });
        
    }
    
    [HttpGet("create")]
    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(int? id)
    {
        _currentUserService.ReloadUserAsync();
        PostCreateViewModel model;

        if (id.HasValue)
        {
            var post = await _context.Posts.FindAsync(id.Value);
            if (post == null)
                return NotFound();

            model = new PostCreateViewModel
            {
                Id = post.Id,
                Text = post.Text,
                ExistingImagePath = post.ImgPath,
            };
        }
        else
        {
            model = new PostCreateViewModel();
        }

        return View(model);
    }

    [HttpPost("publish")]
    [RequestSizeLimit(50 * 1024 * 1024)]
    public async Task<IActionResult> Post(PostCreateViewModel model)
    {
        _currentUserService.ReloadUserAsync();
        
        if (!ModelState.IsValid)
        {
            return View("Edit", model); 
        }

        var domainUser = _currentUserService.EfUser;

        if (domainUser == null)
        {
            return Unauthorized();
        }
        
        if (model.Id != 0)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == model.Id);

            if (post == null)
            {
                throw new ArgumentNullException();
            }
            
            post.Text = model.Text;

            if (model.Image != null)
            {
                var (newPath, newHash) = _fileManagerService.ChangeFile(model.Image, post.ImgPath, post.ImgHash).Result;

                post.ImgPath = newPath;
                post.ImgHash = newHash;
            }
        }
        else
        {
            var (newPath, newHash) = _fileManagerService.SaveFile(model.Image).Result;
            Post post;
            post = new Post
            {
                Author = domainUser,
                AuthorId = domainUser.Id,
                ImgPath = newPath,
                ImgHash = newHash,
                Text = model.Text,
                ReleaseDate = DateTime.Now,
            };
            
            _context.Posts.Add(post);
        }
        
        await _context.SaveChangesAsync();
        
        return RedirectToAction("Account", "Account");
    }

    [HttpPost("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _currentUserService.ReloadUserAsync();
        
        var domainUser = _currentUserService.EfUser;

        if (domainUser == null)
        {
            return Unauthorized();
        }
        
        var postToDelete = _context.Posts.FirstOrDefault(post => post.Id == id);
        
        if (postToDelete == null)
        {
            return NotFound();
        }
        
        var imgPath = postToDelete.ImgPath;

        _context.Posts.Remove(postToDelete);
        
        await _context.SaveChangesAsync();
        
        _fileManagerService.DeleteFile(imgPath);

        return Redirect("/Account");
    }
    
}