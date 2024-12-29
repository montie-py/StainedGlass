using Microsoft.AspNetCore.Mvc;
using StainedGlass.Persistence.Templates;
using StainedGlass.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Web.Controllers.Admin;

[Route("admin/[controller]")]
public class ChurchController : AdminController
{
    private InputBoundary _useCaseInteractor;
    public ChurchController(InputBoundary useCaseInteractor)
    {
        _useCaseInteractor = useCaseInteractor;
        ((UseCaseInteractor)_useCaseInteractor).SetPersistenceTemplate(new DatabasePersistenceTemplate());
    }

    [HttpGet("church")]
    public override IActionResult New()
    {
        return View("Admin/Church/NewChurch");
    }
    
    [HttpGet("churches")]
    public override async Task<IActionResult> All()
    {
        ViewBag.Churches = await _useCaseInteractor.GetAllDTOs<ChurchDTO>();
        return View("Admin/Church/Churches");
    }
    
    [HttpGet("{slug}")]
    public override async Task<IActionResult> One(string slug)
    {
        ViewBag.Church = await _useCaseInteractor.GetDTOBySlug<ChurchDTO>(slug);
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
        return View("Admin/Church/Church");
    }
    
    [HttpGet("{slug}/edit")]
    public override async Task<IActionResult> Edit(string slug)
    {
        ViewBag.Church = await _useCaseInteractor.GetDTOBySlug<ChurchDTO>(slug);
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
        return View("Admin/Church/EditChurch");
    }
    
    [HttpDelete("{slug}")]
    public override async Task<IActionResult> Delete(string slug)
    {
        await _useCaseInteractor.RemoveEntity<ChurchDTO>(slug);
        return Ok();
    }
}