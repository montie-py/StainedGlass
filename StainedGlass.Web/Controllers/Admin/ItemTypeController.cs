using Microsoft.AspNetCore.Mvc;
using StainedGlass.Persistence.Templates;
using StainedGlass.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Web.Controllers.Admin;

[Route("admin/[controller]")]
public class ItemTypeController : AdminController
{
    private InputBoundary _useCaseInteractor;
    public ItemTypeController(InputBoundary useCaseInteractor)
    {
        _useCaseInteractor = useCaseInteractor;
        ((UseCaseInteractor)_useCaseInteractor).SetPersistenceTemplate(new DatabasePersistenceTemplate());
    }
    
    [HttpGet("itemtype")]
    public override async Task<IActionResult> New()
    {
        return View("Admin/ItemType/NewItemType");
    }

    [HttpGet("itemtypes")]
    public override async Task<IActionResult> All()
    {
        ViewBag.ItemTypes = await _useCaseInteractor.GetAllDTOs<ItemTypeDTO>();
        return View("Admin/ItemType/ItemTypes");
    }

    [HttpGet("{slug}")]
    public override async Task<IActionResult> One(string slug)
    {
        ViewBag.ItemType = await _useCaseInteractor.GetDTOBySlug<ItemTypeDTO>(slug);
        return View("Admin/ItemType/ItemType");
    }

    [HttpGet("{slug}/edit")]
    public override async Task<IActionResult> Edit(string slug)
    {
        ViewBag.ItemType = await _useCaseInteractor.GetDTOBySlug<ItemTypeDTO>(slug);
        return View("Admin/ItemType/EditItemType");
    }

    [HttpDelete("{slug}")]
    public override async Task<IActionResult> Delete(string slug)
    {
        await _useCaseInteractor.RemoveEntity<ItemTypeDTO>(slug);
        return Ok();
    }
}