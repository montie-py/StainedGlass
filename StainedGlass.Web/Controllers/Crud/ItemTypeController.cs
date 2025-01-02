using Microsoft.AspNetCore.Mvc;
using StainedGlass.Persistence.Templates;
using StainedGlass.Transfer;
using StainedGlass.Transfer.DTOs;
using StainedGlass.Web.Controllers.Crud;

namespace StainedGlass.Web.Controllers.Crud;

[Route("[controller]")]
// [Route("[controller]/[action]")]
public class ItemTypeController : CrudController
{
    public ItemTypeController(InputBoundary useCaseInteractor)
    {
        _useCaseInteractor = useCaseInteractor;
        ((UseCaseInteractor)_useCaseInteractor).SetPersistenceTemplate(new DatabasePersistenceTemplate());
    }
    
    [HttpGet]
    public override async Task<IActionResult> Get()
    {
        var itemTypes = await _useCaseInteractor.GetAllDTOs<ItemTypeDTO>();
        return Ok(itemTypes);
    }

    [HttpGet("{slug}")]
    public override async Task<IActionResult> Get(string slug)
    {
        var itemType = await _useCaseInteractor.GetDTOBySlug<ItemTypeDTO>(slug);
        return Ok(itemType);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] ItemTypeDTO dto)
    {
        await _useCaseInteractor.StoreEntity(dto);
        return Ok();
    }

    [HttpPut("{originalSlug}")]
    public async Task<IActionResult> Put(string originalSlug, [FromForm] ItemTypeDTO dto)
    {
        await _useCaseInteractor.ReplaceEntity(originalSlug, dto);
        return Ok();
    }

    [HttpDelete("{slug}")]
    public override async Task<IActionResult> Delete(string slug)
    {
        await _useCaseInteractor.RemoveEntity<ItemTypeDTO>(slug);
        return Ok();
    }
}