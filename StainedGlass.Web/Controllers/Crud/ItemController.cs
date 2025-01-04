using Microsoft.AspNetCore.Mvc;
using StainedGlass.Persistence.Templates;
using StainedGlass.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Web.Controllers.Crud;

[Route("[controller]")]
public class ItemController : CrudController
{
    public ItemController(InputBoundary useCaseInteractor) 
    {
        _useCaseInteractor = useCaseInteractor;
        ((UseCaseInteractor)_useCaseInteractor).SetPersistenceTemplate(new DatabasePersistenceTemplate());
    }

    [HttpGet]
    public override async Task<IActionResult> Get()
    {
        var items = await _useCaseInteractor.GetAllDTOs<ItemDTO>();
        return Ok(items);
    }

    [HttpGet("{slug}")]
    public override async Task<IActionResult> Get(string slug)
    {
        var item = await _useCaseInteractor.GetDTOBySlug<ItemDTO>(slug);
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] ItemDTO itemDto)
    {
        foreach (var itemImage in itemDto.ItemImages)
        {
            var fileValidationResult = ValidateFile(itemImage.Image);
            if (fileValidationResult != null)
            {
                ModelState.AddModelError("Image", fileValidationResult);
                return View("Admin/SanctuaryRegion/NewSanctuaryRegion");
            }
        }
        await _useCaseInteractor.StoreEntity(itemDto);
        return Redirect("Admin/Item/Items");
    }

    [HttpPut("{originalSlug}")]
    public async Task<IActionResult> Put(string originalSlug, [FromForm] ItemDTO itemDto)
    {
        await _useCaseInteractor.ReplaceEntity(originalSlug, itemDto);
        return Ok();
    }

    [HttpDelete("{slug}")]
    public override async Task<IActionResult> Delete(string slug)
    {
        await _useCaseInteractor.RemoveEntity<ItemDTO>(slug);
        return Ok();
    }
}