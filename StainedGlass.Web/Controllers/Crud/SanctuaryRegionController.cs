using Microsoft.AspNetCore.Mvc;
using StainedGlass.Persistence.Templates;
using StainedGlass.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Web.Controllers.Crud;

[Route("[controller]")]
public class SanctuaryRegionController : CrudController
{
    public SanctuaryRegionController(InputBoundary useCaseInteractor)
    {
        _useCaseInteractor = useCaseInteractor;
        ((UseCaseInteractor)_useCaseInteractor).SetPersistenceTemplate(new DatabasePersistenceTemplate());
    }

    [HttpGet]
    public override async Task<IActionResult> Get()
    {
        var regions = await _useCaseInteractor.GetAllDTOs<SanctuaryRegionDTO>();
        return Ok(regions);
    }

    [HttpGet("{slug}")]
    public override async Task<IActionResult> Get(string slug)
    {
        var region = await _useCaseInteractor.GetDTOBySlug<SanctuaryRegionDTO>(slug);
        return Ok(region);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] SanctuaryRegionDTO sanctuaryRegionDto)
    {
        var fileValidationResult = ValidateFile((sanctuaryRegionDto as SanctuaryRegionDTO).Image);
        if (fileValidationResult != null)
        {
            ModelState.AddModelError("Image", fileValidationResult);
            return View("Admin/SanctuaryRegion/NewSanctuaryRegion");
        }

        await _useCaseInteractor.StoreEntity(sanctuaryRegionDto);
        return Redirect("Admin/SanctuaryRegion/SanctuaryRegions");
    }

    [HttpPut("{originalSlug}")]
    public async Task<IActionResult> Put(string originalSlug, [FromForm] SanctuaryRegionDTO sanctuaryRegionDto)
    {
        await _useCaseInteractor.ReplaceEntity(originalSlug, sanctuaryRegionDto);
        return Ok();
    }

    [HttpDelete("{slug}")]
    public override async Task<IActionResult> Delete(string slug)
    {
        await _useCaseInteractor.RemoveEntity<SanctuaryRegionDTO>(slug);
        return Ok();
    }
}