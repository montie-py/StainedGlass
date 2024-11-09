using StainedGlass.Entities;
using StainedGlass.Entities.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Transfer.Mapper;

internal class StainedGlassMapper : Mappable
{
    SanctuaryRegionMapper sanctuaryRegionMapper = new();

    public Transferable GetDTO(Entity? entity)
    {
        Entities.StainedGlass stainedGlass = entity as Entities.StainedGlass;
        SanctuaryRegionDTO sanctuaryRegionDTO = sanctuaryRegionMapper.GetDTO(stainedGlass.SanctuaryRegion) as SanctuaryRegionDTO;
        return new StainedGlassDTO
        {
            Slug = stainedGlass.Slug,
            Title = stainedGlass.Title,
            Description = stainedGlass.Description,
            Image = stainedGlass.Image,
            SanctuaryRegion = sanctuaryRegionDTO,
            SanctuaryRegionSlug = sanctuaryRegionDTO.Slug,
        };
    }

    public Transferable GetDTOBySlug(string slug)
    {
        return GetDTO(EntitiesCollection.StainedGlasses.FirstOrDefault(e => e.Slug.Equals(slug)));
    }

    public Entity GetEntity(Transferable transferable)
    {
        StainedGlassDTO stainedGlassDTO = transferable as StainedGlassDTO;

        SanctuaryRegion sanctuaryRegion = EntitiesCollection.SanctuaryRegions.FirstOrDefault(s => s.Slug.Equals(stainedGlassDTO.SanctuaryRegionSlug));

        var window = new Entities.StainedGlass
        {
            Slug = stainedGlassDTO.Slug,
            Title = stainedGlassDTO.Title,
            Description = stainedGlassDTO.Description,
            Image = stainedGlassDTO.Image,
            SanctuaryRegion = sanctuaryRegion
        };

        if (sanctuaryRegion != null) 
        {
            sanctuaryRegion.Windows.Add(window);
        }

        return window;
    }
}