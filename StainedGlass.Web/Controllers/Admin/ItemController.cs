using Microsoft.AspNetCore.Mvc;
using StainedGlass.Persistence.Templates;
using StainedGlass.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Web.Controllers.Admin;

[Route("admin/[controller]")]
public class ItemController : AdminController
{
    private InputBoundary _useCaseInteractor;
    public ItemController(InputBoundary useCaseInteractor)
    {
        _useCaseInteractor = useCaseInteractor;
        ((UseCaseInteractor)_useCaseInteractor).SetPersistenceTemplate(new DatabasePersistenceTemplate());
    }
    
    [HttpGet("item")]
    public override async Task<IActionResult> New()
    {
        ViewBag.SanctuaryRegions = await _useCaseInteractor.GetAllDTOs<SanctuaryRegionDTO>();
        ViewBag.RelatedItems = await _useCaseInteractor.GetAllDTOs<ItemDTO>();
        ViewBag.ItemTypes = await _useCaseInteractor.GetAllDTOs<ItemTypeDTO>();
        
        if (ViewBag.SanctuaryRegions != null)
        {
            ViewBag.SanctuaryRegionsImages = new Dictionary<string, string>();
            foreach (var region in ViewBag.SanctuaryRegions)
            {
                ViewBag.SanctuaryRegionsImages[region.Slug] = await IFormFileToBase64(region.Image);
            }
        }
        return View("Admin/Item/NewItem");
    }

    [HttpGet("items")]
    public override async Task<IActionResult> All()
    {
        ViewBag.Items = await _useCaseInteractor.GetAllDTOs<ItemDTO>();
        return View("Admin/Item/Items");
    }

    [HttpGet("{slug}")]
    public override async Task<IActionResult> One(string slug)
    {
        ViewBag.Item = await _useCaseInteractor.GetDTOBySlug<ItemDTO>(slug);
        ViewBag.ItemImages = new List<string>();
        if (ViewBag.Item.ItemImages.Count > 0) 
        {
            foreach (var itemImage in ViewBag.Item.ItemImages)
            {
                using (var memoryStream = new MemoryStream()) 
                { 
                    await itemImage.Image.CopyToAsync(memoryStream); 
                    var fileBytes = memoryStream.ToArray();
                    ViewBag.ItemImages.Add(Convert.ToBase64String(fileBytes));
                } 
            }
            
        }
        return View("Admin/Item/Item");
    }

    public override async Task<IActionResult> Edit(string slug)
    {
        throw new NotImplementedException();
    }

    public override async Task<IActionResult> Delete(string slug)
    {
        throw new NotImplementedException();
    }
}