namespace StainedGlass.Web.Controllers;

public interface ImageDisplayingInterface
{
    public async Task<string> IFormFileToBase64(IFormFile file)
    {
        using (var memoryStream = new MemoryStream()) 
        { 
            await file.CopyToAsync(memoryStream); 
            var fileBytes = memoryStream.ToArray(); 
            return Convert.ToBase64String(fileBytes); 
        } 
    }
}