﻿using Microsoft.AspNetCore.Mvc;
using StainedGlass.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Web.Controllers;

[Route("[controller]")]
// [Route("[controller]/[action]")]
[ApiController]
public class ItemTypeController : ControllerBase
{
    private InputBoundary _useCaseInteractor;

    public ItemTypeController(InputBoundary useCaseInteractor)
    {
        _useCaseInteractor = useCaseInteractor;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var itemTypes = _useCaseInteractor.GetAllDTOs<ItemTypeDTO>();
        return Ok(itemTypes);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> Get(string slug)
    {
        var itemType = _useCaseInteractor.GetDTOBySlug<ItemTypeDTO>(slug);
        return Ok(itemType);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ItemTypeDTO dto)
    {
        _useCaseInteractor.StoreEntity(dto);
        return Ok();
    }

    [HttpPut("{slug}")]
    public async Task<IActionResult> Put(string slug, [FromBody] ItemTypeDTO dto)
    {
        _useCaseInteractor.ReplaceEntity(slug, dto);
        return Ok();
    }

    [HttpDelete("{slug}")]
    public async Task<IActionResult> Delete(string slug)
    {
        _useCaseInteractor.RemoveEntity<ItemTypeDTO>(slug);
        return Ok();
    }
}