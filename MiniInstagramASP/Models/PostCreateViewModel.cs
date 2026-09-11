using System.ComponentModel.DataAnnotations;
using MiniInstagramEF.entities;

namespace MiniInstagramASP.Models;

public class PostCreateViewModel
{
    public int Id { get; set; } = 0;
    
    [Required(ErrorMessage = "Text příspěvku je povinný.")]
    [StringLength(500, ErrorMessage = "Text příspěvku může mít max {1} znaků.")]
    public string Text { get; set; }
    
    public IFormFile? Image { get; set; }
    
    public string? ExistingImagePath { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(ExistingImagePath) && Image == null)
        {
            yield return new ValidationResult(
                "Musíte nahrát obrázek nebo ponechat existující.",
                new[] { nameof(Image), nameof(ExistingImagePath) });
        }
    }
}