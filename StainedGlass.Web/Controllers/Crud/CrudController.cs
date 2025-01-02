using Microsoft.AspNetCore.Mvc;
using StainedGlass.Transfer.DTOs;
using StainedGlass.Transfer;

namespace StainedGlass.Web.Controllers.Crud;

public abstract class CrudController : Controller
{
    private protected InputBoundary _useCaseInteractor;
    private protected const long maxFileSize = 7 * 1024 * 1024; // 7MB in bytes
    private protected static readonly string[] allowedExtensions = new[] { ".jpg", ".jpeg"};

    public abstract Task<IActionResult> Get();
    public abstract Task<IActionResult> Get(string slug);
    public abstract Task<IActionResult> Delete(string slug);
    
    private protected string? ValidateFile(IFormFile image)
    {
        if (image != null && image.Length > 0)
        {
            //check the image length
            if (image.Length > maxFileSize)
            {
                return "The file size exceeds the 7MB limit.";
            }
            
            //check the image extension
            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                return "Invalid file type.";
            }
        }
        
        return null;
    }
}