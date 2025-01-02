using Microsoft.AspNetCore.Mvc;
using StainedGlass.Persistence.Templates;
using StainedGlass.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Web.Controllers.Crud;

[Route("[controller]")]
public class SanctuarySideController : CrudController
{
    public SanctuarySideController(InputBoundary useCaseInteractor)
    {
        _useCaseInteractor = useCaseInteractor;
        ((UseCaseInteractor)_useCaseInteractor).SetPersistenceTemplate(new DatabasePersistenceTemplate());
    }

    [HttpGet]
    public override async Task<IActionResult> Get()
    {
        var sides = await _useCaseInteractor.GetAllDTOs<SanctuarySideDTO>();
        return Ok(sides);
    }

    [HttpGet("{slug}")]
    public override async Task<IActionResult> Get(string slug)
    {
        var side = await _useCaseInteractor.GetDTOBySlug<SanctuarySideDTO>(slug);
        return Ok(side);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] SanctuarySideDTO sanctuarySideDTO)
    {
        await _useCaseInteractor.StoreEntity(sanctuarySideDTO);
        return Redirect("Admin/SanctuarySide/SanctuarySides");
    }

    [HttpPut("{originalSlug}")]
    public async Task<IActionResult> Put(string originalSlug, [FromForm] SanctuarySideDTO sanctuarySideDTO)
    {
        await _useCaseInteractor.ReplaceEntity(originalSlug, sanctuarySideDTO);
        return Ok();
    }

    [HttpDelete("{slug}")]
    public override async Task<IActionResult> Delete(string slug)
    {
        await _useCaseInteractor.RemoveEntity<SanctuarySideDTO>(slug);
        return Ok();
    }
}