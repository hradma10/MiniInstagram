using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using MiniInstagramASP.Models;
using MiniInstagramASP.Services;
using MiniInstagramEF.context;
using MiniInstagramEF.entities;

namespace MiniInstagramASP.Controllers;


[Route("comment")]
public class CommentController : Controller
{
    private readonly MiniInstagramContext _context;
    private readonly CurrentUserService  _currentUserService;
    
    public CommentController(MiniInstagramContext context, CurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    
    
    
    [HttpPost("add")]
    public async Task<IActionResult> AddComment(CommentViewModel commentModel)
    {
        _currentUserService.ReloadUserAsync();
        
        var currentUser = _currentUserService.EfUser;
        
        if (currentUser == null)
            return Unauthorized();
        
        var post = _context.Posts.Include(p => p.Comments).FirstOrDefault(p => p.Id == commentModel.PostId);
        
        if (post == null)
            return NotFound();

        var comment = new Comment()
        {
            Author = currentUser,
            AuthorId = currentUser.Id,
            Text = commentModel.Text,
            Post = post,
            PostId = commentModel.PostId,
            CreatedAt = DateTime.Now

        };
        
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();
        
        return new PartialViewResult
        {
            ViewName = "_PostComments",
            ViewData = new ViewDataDictionary(
                metadataProvider: new EmptyModelMetadataProvider(),
                modelState: ViewData.ModelState)
            {
                Model = post,
                ["CurrentUserId"] = currentUser.Id,
                ["WithComments"] = true,
                ["Disabled"] = false
            }
        };
    }
    
    [HttpPost("remove/{id}")]
    public async Task<IActionResult> RemoveComment(int id)
    {
        _currentUserService.ReloadUserAsync();
        
        var currentUser = _currentUserService.EfUser;
        
        if  (currentUser == null)
            return Unauthorized();
        
        var commentToDelete = _context.Comments.Include(comment => comment.Author).Include(c=> c.Post).FirstOrDefault(p => p.Id == id);
        if (commentToDelete == null)
        {
            return NotFound();
        }

        if (commentToDelete.Author != currentUser)
        {
            return Unauthorized();
        }

        var post = _context.Posts.Include(p => p.Comments).FirstOrDefault(p => p.Id == commentToDelete.PostId);
        
        _context.Comments.Remove(commentToDelete);
        await _context.SaveChangesAsync();
        
        return new PartialViewResult
        {
            ViewName = "_PostComments",
            ViewData = new ViewDataDictionary(
                metadataProvider: new EmptyModelMetadataProvider(),
                modelState: ViewData.ModelState)
            {
                Model = post,
                ["CurrentUserId"] = currentUser.Id,
                ["WithComments"] = true,
                ["Disabled"] = false
            }
        };
    }
    
    [HttpPost("comment/like/{id}")]
    public async Task<IActionResult> ToggleCommentLike(int id)
    {
        _currentUserService.ReloadUserAsync();
        
        var currentUser = _currentUserService.EfUser;
        
        if (currentUser == null)
        {
            return Unauthorized();
        }
        
        var comment = _context.Comments.Include(post => post.LikedBy).Include(c=> c.Post).FirstOrDefaultAsync(p => p.Id == id).Result;

        if (comment == null)
        {
            return NotFound();
        }
        
        bool likedComment;
        if (comment.LikedBy.Contains(currentUser))
        {
            comment.LikedBy.Remove(currentUser);
            likedComment = false;
        }
        else
        {
            comment.LikedBy.Add(currentUser);
            likedComment = true;
        }
        
        await _context.SaveChangesAsync();
        
        return new JsonResult(new
        {
            liked = likedComment,
            likesCount = comment.LikedBy.Count
        });
    }
}