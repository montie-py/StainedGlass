using StainedGlass.Persistence.Transfer;
using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Persistence.Services.Entities;

public class EntitySanctuarySide : INonRelatable, IPersistenceService
{
    public IEnumerable<Transferable?> GetAllDTOs()
    {
        return EntitiesCollection.SanctuarySides.Select(
            e => GetDTO(e.Value, skipParentElements: true)
        );
    }
    
    public Transferable? GetDTOBySlug(string slug)
    {
        if (!EntitiesCollection.SanctuarySides.ContainsKey(slug))
        {
            return null;
        }
        return GetDTO(EntitiesCollection.SanctuarySides[slug], skipParentElements: true);
    }

    public void AddEntity(IPersistanceTransferStruct transferStruct)
    {
        EntitiesCollection.SanctuarySides.TryAdd(Slug, this);
    }

    public IEnumerable<IPersistanceTransferStruct> GetAllDtos()
    {
        throw new NotImplementedException();
    }

    public IPersistanceTransferStruct? GetDto(string slug)
    {
        throw new NotImplementedException();
    }

    public void RemoveEntity(string slug)
    {
        if (!EntitiesCollection.SanctuarySides.ContainsKey(slug))
        {
           return;  
        }
        
        //remove the side from its church
        var church = EntitiesCollection.Churches.Values.FirstOrDefault(e => 
            e.Sides?.FirstOrDefault(s => s.Slug == Slug) != null
        );
        if (church != null)
        {
            church.Sides?.RemoveWhere(e => e.Slug == Slug);
        }
        EntitiesCollection.SanctuarySides.Remove(Slug);
    }

    public void ReplaceEntity(string slug, IPersistanceTransferStruct transferStruct)
    {
        if (!EntitiesCollection.SanctuarySides.ContainsKey(slug))
        {
            return;
        }
        entity.Slug = slug;
        var oldEntity = EntitiesCollection.SanctuarySides[slug];
        EntitiesCollection.SanctuarySides[slug] = (SanctuarySide)entity;
        
        //if old entity has an assigned church - new one cannot lack one
        if (EntitiesCollection.SanctuarySides[slug].Church is null)
        {
            EntitiesCollection.SanctuarySides[slug].Church = oldEntity.Church;
        }
        
        EntitiesCollection.SanctuarySides[slug].Regions = oldEntity.Regions;
    }

    public Transferable? GetDTO(Entity? entity, bool skipParentElements = false, bool skipChildrenElements = false)
    {
        if (entity == null) 
        {
            return null;
        }
        
        ChurchMapper churchMapper = new();
        SanctuaryRegionMapper sanctuaryRegionMapper = new();
        SanctuarySide? sanctuarySide = entity as SanctuarySide;

        List<SanctuaryRegionDTO> regionsDTO = new();
        ChurchDTO churchDTO = null;
        
        if (!skipChildrenElements)
        {
            foreach (SanctuaryRegion? region in sanctuarySide.Regions)
            {
                regionsDTO.Add(sanctuaryRegionMapper.GetDTO(
                    region, skipParentElements: true, skipChildrenElements: true
                ) as SanctuaryRegionDTO);
            }
        }

        var sanctuarySideDTO = new SanctuarySideDTO
        {
            Slug = sanctuarySide.Slug,
            Name = sanctuarySide.Name,
            Regions = regionsDTO,
            Church = churchDTO
        };

        if (!skipParentElements)
        {
            churchDTO = churchMapper.GetDTO(sanctuarySide.Church) as ChurchDTO;
            if (churchDTO != null)
            {
                sanctuarySideDTO.ChurchSlug = churchDTO.Slug;
            }
        }
        
        return sanctuarySideDTO;
    }

    public Entity GetEntity(Transferable transferable)
    {
        SanctuarySideDTO sanctuarySideDTO = transferable as SanctuarySideDTO;

        Church sanctuarySideChurch = 
            (
                sanctuarySideDTO.ChurchSlug != null 
                && EntitiesCollection.Churches.ContainsKey(sanctuarySideDTO.ChurchSlug)
                ) ? EntitiesCollection.Churches[sanctuarySideDTO.ChurchSlug] : null;

        SanctuarySide sanctuarySide = new SanctuarySide
        {
            Name = sanctuarySideDTO.Name,
            Slug = sanctuarySideDTO.Slug,
            Regions = null,
            Church = sanctuarySideChurch,
        };

        if (sanctuarySideDTO.Regions != null)
        {
            SanctuaryRegionMapper sanctuaryRegionMapper = new();
            sanctuarySide.Regions =
                sanctuarySideDTO.Regions.Select(
                    e => sanctuaryRegionMapper.GetEntity(e) as SanctuaryRegion
                    ).ToList();
        }

        if (sanctuarySideChurch != null) {
            sanctuarySideChurch.Sides.Add(sanctuarySide);
        }

        return sanctuarySide;
    }
}