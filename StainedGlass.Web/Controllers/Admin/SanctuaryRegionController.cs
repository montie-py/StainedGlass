using Microsoft.AspNetCore.Mvc;
using StainedGlass.Persistence.Templates;
using StainedGlass.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Web.Controllers.Admin;

[Route("admin/[controller]")]
public class SanctuaryRegionController : AdminController
{
    private InputBoundary _useCaseInteractor;
    public SanctuaryRegionController(InputBoundary useCaseInteractor)
    {
        _useCaseInteractor = useCaseInteractor;
        ((UseCaseInteractor)_useCaseInteractor).SetPersistenceTemplate(new DatabasePersistenceTemplate());
    }
    
    [HttpGet("sanctuaryregion")]
    public override async Task<IActionResult> New()
    {
        ViewBag.SanctuarySides = await _useCaseInteractor.GetAllDTOs<SanctuarySideDTO>(); 
        return View("Admin/SanctuaryRegion/NewSanctuaryRegion");
    }

    [HttpGet("sanctuaryregions")]
    public override async Task<IActionResult> All()
    {
        ViewBag.SanctuaryRegions = await _useCaseInteractor.GetAllDTOs<SanctuaryRegionDTO>();
        return View("Admin/SanctuaryRegion/SanctuaryRegions");
    }

    [HttpGet("{slug}")]
    public override async Task<IActionResult> One(string slug)
    {
        ViewBag.SanctuaryRegion = await _useCaseInteractor.GetDTOBySlug<SanctuaryRegionDTO>(slug);
        if (ViewBag.SanctuaryRegion.Image != null && ViewBag.SanctuaryRegion.Image.Length > 0) 
        { 
            ViewBag.SanctuaryRegionImage = await IFormFileToBase64(ViewBag.SanctuaryRegion.Image);
        }
        return View("Admin/SanctuaryRegion/SanctuaryRegion");
    }
    
    [HttpGet("{slug}/edit")]
    public override async Task<IActionResult> Edit(string slug)
    {
        ViewBag.SanctuarySides = await _useCaseInteractor.GetAllDTOs<SanctuarySideDTO>();
        ViewBag.SanctuaryRegion = await _useCaseInteractor.GetDTOBySlug<SanctuaryRegionDTO>(slug);
        if (ViewBag.SanctuaryRegion.Image != null && ViewBag.SanctuaryRegion.Image.Length > 0) 
        { 
            ViewBag.SanctuaryRegionImage = IFormFileToBase64(ViewBag.SanctuaryRegion.Image);
        }
        return View("Admin/SanctuaryRegion/EditSanctuaryRegion");
    }

    [HttpDelete("{slug}")]
    public override async Task<IActionResult> Delete(string slug)
    {
        await _useCaseInteractor.RemoveEntity<SanctuaryRegionDTO>(slug);
        return Ok();
    }
}