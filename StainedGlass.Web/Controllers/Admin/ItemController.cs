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
                    await itemImage.CopyToAsync(memoryStream); 
                    var fileBytes = memoryStream.ToArray();
                    ViewBag.ItemImages.Add(Convert.ToBase64String(fileBytes));
                } 
            }
            
        }
        if (ViewBag.Item.SanctuaryRegion.Image != null && ViewBag.Item.SanctuaryRegion.Image.Length > 0)
        {
            ViewBag.SanctuaryRegionImage = await IFormFileToBase64(ViewBag.Item.SanctuaryRegion.Image);
        }
        return View("Admin/Item/Item");
    }

    [HttpGet("{slug}/edit")]
    public override async Task<IActionResult> Edit(string slug)
    {
        var item = await _useCaseInteractor.GetDTOBySlug<ItemDTO>(slug);
        ViewBag.Item = item;
        ViewBag.SanctuaryRegions = await _useCaseInteractor.GetAllDTOs<SanctuaryRegionDTO>();
        ViewBag.RelatedItems = await _useCaseInteractor.GetAllDTOs<ItemDTO>();
        ViewBag.ItemTypes = await _useCaseInteractor.GetAllDTOs<ItemTypeDTO>();
        
        ViewBag.ItemImages = new Dictionary<string, string>();
        if (ViewBag.Item.ItemImages.Count > 0) 
        {
            foreach (var itemImage in ViewBag.Item.ItemImages)
            {
                using (var memoryStream = new MemoryStream()) 
                { 
                    await itemImage.CopyToAsync(memoryStream); 
                    var fileBytes = memoryStream.ToArray();
                    ViewBag.ItemImages.Add(itemImage.FileName, Convert.ToBase64String(fileBytes));
                } 
            }
            
        }
        
        if (item.SanctuaryRegion.Image != null && item.SanctuaryRegion.Image.Length > 0)
        {
            ViewBag.SanctuaryRegionImage = await IFormFileToBase64(item.SanctuaryRegion.Image);
        }
        
        if (ViewBag.SanctuaryRegions != null)
        {
            ViewBag.SanctuaryRegionsImages = new Dictionary<string, string>();
            foreach (var region in ViewBag.SanctuaryRegions)
            {
                ViewBag.SanctuaryRegionsImages[region.Slug] = await IFormFileToBase64(region.Image);
            }
        }
        
        ViewBag.RelatedItemsSlugs = new List<string>();
        if (item.RelatedItems.Count > 0)
        {
            ViewBag.RelatedItemsSlugs = item.RelatedItems.Keys.ToList();
        }
        return View("Admin/Item/EditItem");
    }

    public override async Task<IActionResult> Delete(string slug)
    {
        throw new NotImplementedException();
    }
}