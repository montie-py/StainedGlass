using Microsoft.AspNetCore.Mvc;
using StainedGlass.Persistence.Templates;
using StainedGlass.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Web.Controllers;

[Route("[controller]")]
public class ChurchController : ControllerBase
{
    private InputBoundary _useCaseInteractor;
    
    public ChurchController(InputBoundary useCaseInteractor)
    {
        _useCaseInteractor = useCaseInteractor;
        ((UseCaseInteractor)_useCaseInteractor).SetPersistenceTemplate(new DatabasePersistenceTemplate());
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var churches = _useCaseInteractor.GetAllDTOs<ChurchDTO>();
        return Ok(churches);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> Get(string slug)
    {
        var church = _useCaseInteractor.GetDTOBySlug<ChurchDTO>(slug);
        return Ok(church);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm]ChurchDTO churchDto)
    {
        //file handling
        using (var memoryStream = new MemoryStream())
        {
            // await 
        }
        
        _useCaseInteractor.StoreEntity(churchDto);
        return Ok();
    }

    [HttpPut("{originalSlug}")]
    public async Task<IActionResult> Put(string originalSlug, [FromForm]ChurchDTO church)
    {
        _useCaseInteractor.ReplaceEntity(originalSlug, church);
        return Ok();
    }

    [HttpDelete("{slug}")]
    public async Task<IActionResult> Delete(string slug)
    {
        _useCaseInteractor.RemoveEntity<ChurchDTO>(slug);
        return Ok();
    }
}