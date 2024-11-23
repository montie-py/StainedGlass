using Microsoft.AspNetCore.Mvc;
using StainedGlass.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Web.Controllers;

[Route("[controller]")]
[ApiController]
public class SanctuaryRegionController : ControllerBase
{
    private InputBoundary _useCaseInteractor;

    public SanctuaryRegionController(InputBoundary useCaseInteractor)
    {
        _useCaseInteractor = useCaseInteractor;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var regions = _useCaseInteractor.GetAllDTOs<SanctuaryRegionDTO>();
        return Ok(regions);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> Get(string slug)
    {
        var region = _useCaseInteractor.GetDTOBySlug<SanctuaryRegionDTO>(slug);
        return Ok(region);
    }

    [HttpPost]
    public async Task<IActionResult> Post(SanctuaryRegionDTO region)
    {
        _useCaseInteractor.StoreEntity(region);
        return Ok();
    }

    [HttpPut("{slug}")]
    public async Task<IActionResult> Put(string slug, SanctuaryRegionDTO region)
    {
        _useCaseInteractor.ReplaceEntity(slug, region);
        return Ok();
    }

    [HttpDelete("{slug}")]
    public async Task<IActionResult> Delete(string slug)
    {
        _useCaseInteractor.RemoveEntity<SanctuaryRegionDTO>(slug);
        return Ok();
    }
}