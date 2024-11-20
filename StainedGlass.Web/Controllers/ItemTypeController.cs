using Microsoft.AspNetCore.Mvc;
using StainedGlass.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Web.Controllers;

[Route("[controller]")]
// [Route("[controller]/[action]")]
[ApiController]
public class ItemTypeController
{
    private InputBoundary useCaseInteractor;
    
    public ItemTypeController()
    {
        useCaseInteractor = new UseCaseInteractor();
    }

    [HttpGet]
    public IEnumerable<ItemTypeDTO> Get()
    {
        
        return useCaseInteractor.GetAllDTOs<ItemTypeDTO>();
    }

    [HttpPost]
    public void Post([FromBody] ItemTypeDTO dto)
    {
        useCaseInteractor.StoreEntity(dto);
    }

    [HttpPut("{slug}")]
    public void Put(string slug, [FromBody] ItemTypeDTO dto)
    {
        useCaseInteractor.ReplaceEntity(slug, dto);
    }

    [HttpDelete("{slug}")]
    public void Delete(string slug)
    {
        useCaseInteractor.RemoveEntity<ItemTypeDTO>(slug);
    }
}