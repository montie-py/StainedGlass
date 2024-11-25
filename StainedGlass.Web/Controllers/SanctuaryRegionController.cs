using Microsoft.AspNetCore.Mvc;
using StainedGlass.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Web.Controllers;

[Route("[controller]")]
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
    public async Task<IActionResult> Post([FromForm] SanctuaryRegionDTO region)
    {
        _useCaseInteractor.StoreEntity(region);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Put(string slug,[FromForm] SanctuaryRegionDTO region)
    {
        _useCaseInteractor.ReplaceEntity(slug, region);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(string slug)
    {
        _useCaseInteractor.RemoveEntity<SanctuaryRegionDTO>(slug);
        return Ok();
    }
}