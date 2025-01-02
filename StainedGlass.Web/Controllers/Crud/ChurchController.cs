using Microsoft.AspNetCore.Mvc;
using StainedGlass.Persistence.Templates;
using StainedGlass.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Web.Controllers.Crud;

[Route("[controller]")]
public class ChurchController : CrudController
{
    public ChurchController(InputBoundary useCaseInteractor)
    {
        _useCaseInteractor = useCaseInteractor;
        ((UseCaseInteractor)_useCaseInteractor).SetPersistenceTemplate(new DatabasePersistenceTemplate());
    }

    [HttpGet]
    public override async Task<IActionResult> Get()
    {
        var churches = await _useCaseInteractor.GetAllDTOs<ChurchDTO>();
        return Ok(churches);
    }

    [HttpGet("{slug}")]
    public override async Task<IActionResult> Get(string slug)
    {
        var church = await _useCaseInteractor.GetDTOBySlug<ChurchDTO>(slug);
        return Ok(church);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm]ChurchDTO churchDto)
    {
        var fileValidationResult = ValidateFile((churchDto as ChurchDTO).Image);
        if (fileValidationResult != null)
        {
            ModelState.AddModelError("Image", fileValidationResult);
            return View("Admin/Church/NewChurch");
        }

        await _useCaseInteractor.StoreEntity(churchDto);
        return Redirect("Admin/Church/churches");
    }

    [HttpPut("{originalSlug}")]
    public async Task<IActionResult> Put(string originalSlug, ChurchDTO churchDto)
    {
        var fileValidationResult = ValidateFile((churchDto as ChurchDTO).Image);
        if (fileValidationResult != null)
        {
            ModelState.AddModelError("Image", fileValidationResult);
            return View("Admin/Church/EditChurch");
        }
        
        await _useCaseInteractor.ReplaceEntity(originalSlug, churchDto);
        return Ok();
    }
    
    [HttpDelete("{slug}")]
    public override async Task<IActionResult> Delete(string slug)
    {
        await _useCaseInteractor.RemoveEntity<ChurchDTO>(slug);
        return Ok();
    }
}