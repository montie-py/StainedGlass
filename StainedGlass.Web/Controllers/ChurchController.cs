﻿using Microsoft.AspNetCore.Mvc;
using StainedGlass.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Web.Controllers;

[Route("[controller]")]
[ApiController]
public class ChurchController : ControllerBase
{
    private InputBoundary _useCaseInteractor;
    
    public ChurchController(InputBoundary useCaseInteractor)
    {
        _useCaseInteractor = useCaseInteractor;
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
    public async Task<IActionResult> Post(ChurchDTO churchDto)
    {
        _useCaseInteractor.StoreEntity(churchDto);
        return Ok();
    }

    [HttpPut("{slug}")]
    public async Task<IActionResult> Put(string slug, ChurchDTO church)
    {
        _useCaseInteractor.ReplaceEntity(slug, church);
        return Ok();
    }

    [HttpDelete("{slug}")]
    public async Task<IActionResult> Delete(string slug)
    {
        _useCaseInteractor.RemoveEntity<ChurchDTO>(slug);
        return Ok();
    }
}