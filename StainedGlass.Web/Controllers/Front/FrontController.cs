using Microsoft.AspNetCore.Mvc;
using StainedGlass.Persistence.Templates;
using StainedGlass.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Web.Controllers.Front;

[Route("[controller]")]
public class FrontController : Controller, ImageDisplayingInterface
{
    private protected InputBoundary _useCaseInteractor;

    public FrontController(InputBoundary useCaseInteractor)
    {
        _useCaseInteractor = useCaseInteractor;
        ((UseCaseInteractor)_useCaseInteractor).SetPersistenceTemplate(new DatabasePersistenceTemplate());
    }

    [HttpGet("sanctuaryside/{slug}")]
    public async Task<IActionResult> SanctuaryRegionsBySide(string slug)
    {
        var includeChildrenToTheResponse = true;
        ViewBag.SanctuarySide = await _useCaseInteractor.GetDTOBySlug<SanctuarySideDTO>(slug, includeChildrenToTheResponse);
        ViewBag.SanctuaryRegionImages = new Dictionary<string, string>();
        if (ViewBag.SanctuarySide.Regions != null && ViewBag.SanctuarySide.Regions.Count > 0)
        {
            foreach (var sanctuaryRegion in ViewBag.SanctuarySide.Regions)
            {
                ViewBag.SanctuaryRegionImages[sanctuaryRegion.Slug] = await 
                    ((ImageDisplayingInterface)this).IFormFileToBase64(sanctuaryRegion.Image);
            }   
        }
        
        return PartialView("_SanctuaryRegions");
    }

    [HttpGet("sanctuaryregion/{slug}")]
    public async Task<IActionResult> ItemsByRegion(string slug)
    {
        var includeChildrenToTheResponse = true;
        ViewBag.SanctuaryRegion = await _useCaseInteractor.GetDTOBySlug<SanctuaryRegionDTO>(slug, includeChildrenToTheResponse);
        ViewBag.SanctuaryRegionImage = await ((ImageDisplayingInterface)this).IFormFileToBase64(ViewBag.SanctuaryRegion.Image);
        return PartialView("_Items");
    }
}