using Microsoft.AspNetCore.Mvc;

namespace StainedGlass.Web.Controllers.Admin;

public abstract class AdminController : Controller
{
    public abstract Task<IActionResult> New();
    public abstract Task<IActionResult> All();
    public abstract Task<IActionResult> One(string slug);
    public abstract Task<IActionResult> Edit(string slug);
    public abstract Task<IActionResult> Delete(string slug);

    private protected async Task<string> IFormFileToBase64(IFormFile file)
    {
        using (var memoryStream = new MemoryStream()) 
        { 
            await file.CopyToAsync(memoryStream); 
            var fileBytes = memoryStream.ToArray(); 
            return Convert.ToBase64String(fileBytes); 
        } 
    }
}