using Microsoft.AspNetCore.Mvc;
using StainedGlass.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Web.Controllers;

[Route("[controller]")]
public class SanctuarySideController : ControllerBase
{
    private InputBoundary _useCaseInteractor;
    
    public SanctuarySideController(InputBoundary useCaseInteractor)
    {
        _useCaseInteractor = useCaseInteractor;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var sides = _useCaseInteractor.GetAllDTOs<SanctuarySideDTO>();
        return Ok(sides);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> Get(string slug)
    {
        var side = _useCaseInteractor.GetDTOBySlug<SanctuarySideDTO>(slug);
        return Ok(side);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] SanctuarySideDTO side)
    {
        _useCaseInteractor.StoreEntity(side);
        return Ok();
    }

    [HttpPut("{originalSlug}")]
    public async Task<IActionResult> Put(string originalSlug, [FromForm] SanctuarySideDTO side)
    {
        _useCaseInteractor.ReplaceEntity(originalSlug, side);
        return Ok();
    }

    [HttpDelete("{slug}")]
    public async Task<IActionResult> Delete(string slug)
    {
        _useCaseInteractor.RemoveEntity<SanctuarySideDTO>(slug);
        return Ok();
    }
}