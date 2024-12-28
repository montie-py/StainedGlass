using Microsoft.AspNetCore.Mvc;
using StainedGlass.Persistence.Templates;
using StainedGlass.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Web.Controllers;

[Route("[controller]")]
public class AdminController : Controller
{
    private InputBoundary _useCaseInteractor;
    public AdminController(InputBoundary useCaseInteractor)
    {
        _useCaseInteractor = useCaseInteractor;
        ((UseCaseInteractor)_useCaseInteractor).SetPersistenceTemplate(new DatabasePersistenceTemplate());
    }

    [HttpGet("church")]
    public IActionResult NewChurch()
    {
        return View("NewChurch");
    }
    
    [HttpGet("churches")]
    public IActionResult Churches()
    {
        ViewBag.Churches = _useCaseInteractor.GetAllDTOs<ChurchDTO>();
        return View();
    }
    
    [HttpGet("church/{slug}")]
    public async Task<IActionResult> Get(string slug)
    {
        ViewBag.Church = _useCaseInteractor.GetDTOBySlug<ChurchDTO>(slug);
        if (ViewBag.Church.Image != null && ViewBag.Church.Image.Length > 0) 
        { 
            using (var memoryStream = new MemoryStream()) 
            { 
                await ViewBag.Church.Image.CopyToAsync(memoryStream); 
                var fileBytes = memoryStream.ToArray(); 
                var base64String = Convert.ToBase64String(fileBytes);
                ViewBag.ChurchImage = base64String; 
            } 
        }
        return View("Church");
    }
    
    [HttpGet("church/{slug}/edit")]
    public async Task<IActionResult> EditChurch(string slug)
    {
        ViewBag.Church = _useCaseInteractor.GetDTOBySlug<ChurchDTO>(slug);
        if (ViewBag.Church.Image != null && ViewBag.Church.Image.Length > 0) 
        { 
            using (var memoryStream = new MemoryStream()) 
            { 
                await ViewBag.Church.Image.CopyToAsync(memoryStream); 
                var fileBytes = memoryStream.ToArray(); 
                var base64String = Convert.ToBase64String(fileBytes);
                ViewBag.ChurchImage = base64String; 
            } 
        }
        return View("EditChurch");
    }
    
    [HttpDelete("church/{slug}")]
    public async Task<IActionResult> Delete(string slug)
    {
        _useCaseInteractor.RemoveEntity<ChurchDTO>(slug);
        return Ok();
    }
}