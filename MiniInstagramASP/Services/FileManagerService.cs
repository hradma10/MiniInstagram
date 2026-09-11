using System.Security.Cryptography;

namespace MiniInstagramASP.Services;

public class FileManagerService
{
    
    private readonly IConfiguration _config;
    
    public FileManagerService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<(string, string)> ChangeFile(IFormFile file, string savePath, string prevFileHash)
    {
        var hashBytes = await SHA256.Create().ComputeHashAsync(file.OpenReadStream());
        
        var newFileHash = Convert.ToBase64String(hashBytes);

        if (prevFileHash == newFileHash)
        {
            return (savePath, prevFileHash);
        }
        
        DeleteFile(savePath);
        
        return await SaveFile(file);
        
    }

    public void DeleteFile(string imgPath)
    {
        var savePath = _config["Paths:SaveFile"];

        if (savePath == null)
        {
            throw new ArgumentNullException(nameof(savePath));
        }
        
        var newFilePath = Path.Combine(savePath, imgPath);

        try
        {
            File.Delete(newFilePath);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        
    }
    
    public async Task<(string, string)> SaveFile(IFormFile file, string hash = "")
    {
        var savePath = _config["Paths:SaveFile"];

        if (savePath == null)
        {
            throw new ArgumentNullException(nameof(savePath));
        }
        
        var newFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        
        var newFilePath = Path.Combine(savePath, newFileName);

        if (hash == "")
        {
            var hashBytes = await SHA256.Create().ComputeHashAsync(file.OpenReadStream());
        
            hash = Convert.ToBase64String(hashBytes);
        }

        Directory.CreateDirectory(savePath);
        
        await using var stream = new FileStream(newFilePath, FileMode.Create);
        
        await file.CopyToAsync(stream);

        return (newFileName, hash);
    }
}