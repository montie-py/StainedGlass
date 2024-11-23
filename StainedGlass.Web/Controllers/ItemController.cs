using Microsoft.AspNetCore.Mvc;
using StainedGlass.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Web.Controllers;

[Route("[controller]")]
[ApiController]
public class ItemController : ControllerBase
{
    private InputBoundary _useCaseInteractor;
    public ItemController(InputBoundary useCaseInteractor) 
    {
        _useCaseInteractor = useCaseInteractor;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var items = _useCaseInteractor.GetAllDTOs<ItemDTO>();
        return Ok(items);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> Get(string slug)
    {
        var item = _useCaseInteractor.GetDTOBySlug<ItemDTO>(slug);
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ItemDTO item)
    {
        _useCaseInteractor.StoreEntity(item);
        return Ok();
    }

    [HttpPut("{slug}")]
    public async Task<IActionResult> Put(string slug, [FromBody] ItemDTO item)
    {
        _useCaseInteractor.ReplaceEntity(slug, item);
        return Ok();
    }

    [HttpDelete("{slug}")]
    public async Task<IActionResult> Delete(string slug)
    {
        _useCaseInteractor.RemoveEntity<ItemDTO>(slug);
        return Ok();
    }
}