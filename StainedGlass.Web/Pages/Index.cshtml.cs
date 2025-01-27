using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StainedGlass.Persistence.Templates;
using StainedGlass.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Web.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private protected InputBoundary _useCaseInteractor;
    
    public ChurchDTO? Church { get; set; }
    public string ChurchImage { get; set; }

    public IndexModel(InputBoundary useCaseInteractor, ILogger<IndexModel> logger)
    {
        _logger = logger;
        _useCaseInteractor = useCaseInteractor;
        ((UseCaseInteractor)_useCaseInteractor).SetPersistenceTemplate(new DatabasePersistenceTemplate());
    }

    public async void OnGet()
    {
        Church = await _useCaseInteractor.GetDTOBySlug<ChurchDTO>("knoxunited");
        ChurchImage = await IFormFileToBase64(Church.Image);
    }
    
    private async Task<string> IFormFileToBase64(IFormFile file)
    {
        using (var memoryStream = new MemoryStream()) 
        { 
            await file.CopyToAsync(memoryStream); 
            var fileBytes = memoryStream.ToArray(); 
            return Convert.ToBase64String(fileBytes); 
        } 
    }
}
