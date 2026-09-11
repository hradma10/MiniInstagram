using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniInstagramASP.Data;
using MiniInstagramASP.Services;
using MiniInstagramEF.context;

namespace MiniInstagramASP.Controllers;

[Route("image")]
public class ImageController: Controller
{
    
    private readonly IConfiguration _config;

    public ImageController(IConfiguration config)
    {
        _config = config;
    }
    
    [HttpGet("{filename}")]
    public async Task<IActionResult> Get(string filename)
    {
        
        var root = _config["Paths:SaveFile"];

        if (root == null)
        {
            return NotFound();
        }
        
        var fullPath = Path.Combine(root, filename);
        
        var fileBytes = await System.IO.File.ReadAllBytesAsync(fullPath);

        var ext = Path.GetExtension(filename).ToLowerInvariant();
        var contentType = ext switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            _ => "application/octet-stream"
        };

        return File(fileBytes, contentType);
    }
}