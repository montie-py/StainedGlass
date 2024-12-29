using Microsoft.AspNetCore.Mvc;
using StainedGlass.Persistence.Templates;
using StainedGlass.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Web.Controllers;

[Route("[controller]")]
public class ChurchController : Controller
{
    private InputBoundary _useCaseInteractor;
    
    private const long maxFileSize = 7 * 1024 * 1024; // 7MB in bytes
    private static readonly string[] allowedExtensions = new[] { ".jpg", ".jpeg"};

    public ChurchController(InputBoundary useCaseInteractor)
    {
        _useCaseInteractor = useCaseInteractor;
        ((UseCaseInteractor)_useCaseInteractor).SetPersistenceTemplate(new DatabasePersistenceTemplate());
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var churches = _useCaseInteractor.GetAllDTOs<ChurchDTO>();
        return Ok(churches);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> Get(string slug)
    {
        var church = _useCaseInteractor.GetDTOBySlug<ChurchDTO>(slug);
        return Ok(church);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm]ChurchDTO churchDto)
    {
        var fileValidationResult = ValidateFile(churchDto.Image);
        if (fileValidationResult != null)
        {
            ModelState.AddModelError("Image", fileValidationResult);
            return View("Admin/NewChurch");
        }

        await _useCaseInteractor.StoreEntity(churchDto);
        return Redirect("Admin/churches");
    }

    [HttpPut("{originalSlug}")]
    public async Task<IActionResult> Put(string originalSlug, ChurchDTO churchDto)
    {
        var fileValidationResult = ValidateFile(churchDto.Image);
        if (fileValidationResult != null)
        {
            ModelState.AddModelError("Image", fileValidationResult);
            return View("Admin/EditChurch");
        }
        
        await _useCaseInteractor.ReplaceEntity(originalSlug, churchDto);
        return Ok();
    }

    private string? ValidateFile(IFormFile image)
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