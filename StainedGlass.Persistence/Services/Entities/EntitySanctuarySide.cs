using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;
using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Persistence.Services.Entities;

public class EntitySanctuarySide : INonRelatable, IPersistenceService
{
    public void AddEntity(IPersistanceTransferStruct transferStruct)
    {
        var sanctuarySideDto = (SanctuarySideDTO)transferStruct;
        var entity = GetEntity(sanctuarySideDto) as SanctuarySide;
        EntitiesCollection.SanctuarySides.TryAdd(sanctuarySideDto.Slug, entity);
    } 
    
    public IEnumerable<IPersistanceTransferStruct> GetAllDtos()
    {
        return EntitiesCollection.SanctuarySides.Select(
            e => GetDTOForEntity(e.Value, skipParentElements: true)
        );
    }
    
    public IPersistanceTransferStruct? GetDTOForEntity(
        IEntity entity, 
        bool skipParentElements = false, 
        bool skipChildrenElements = false
    )
    {
        if (entity == null) 
        {
            return null;
        }
        
        EntityChurch entityChurch = new();
        EntitySanctuaryRegion entitySanctuaryRegion = new();
        SanctuarySide? sanctuarySide = entity as SanctuarySide;

        List<SanctuaryRegionDTO> regionsDTO = new();
        ChurchDTO churchDTO = new();
        
        if (!skipChildrenElements)
        {
            foreach (SanctuaryRegion? region in sanctuarySide.Regions)
            {
                regionsDTO.Add((SanctuaryRegionDTO)entitySanctuaryRegion.GetDTOForEntity(
                    region, skipParentElements: true, skipChildrenElements: true
                ));
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
            var nullableChurchDTO = entityChurch.GetDTOForEntity(sanctuarySide.Church) as ChurchDTO?;
            if (nullableChurchDTO != null)
            {
                churchDTO = (ChurchDTO)nullableChurchDTO;
                sanctuarySideDTO.Church = churchDTO;
                sanctuarySideDTO.ChurchSlug = churchDTO.Slug;
            }
        }
        
        return sanctuarySideDTO;
    }

    public IPersistanceTransferStruct? GetDtoBySlug(string slug)
    {
        if (!EntitiesCollection.SanctuarySides.ContainsKey(slug))
        {
            return null;
        }
        return GetDTOForEntity(EntitiesCollection.SanctuarySides[slug], skipParentElements: true);
    }

    public void RemoveEntity(string slug)
    {
        if (!EntitiesCollection.SanctuarySides.ContainsKey(slug))
        {
           return;  
        }
        
        //remove the side from its church
        var church = EntitiesCollection.Churches.Values.FirstOrDefault(e => 
            e.SanctuarySides?.FirstOrDefault(s => s.Slug == slug) != null
        );
        if (church != null)
        {
            ((HashSet<SanctuarySide>)church.SanctuarySides).RemoveWhere(e => e.Slug == slug);
        }
        EntitiesCollection.SanctuarySides.Remove(slug);
    }

    public void ReplaceEntity(string slug, IPersistanceTransferStruct transferStruct)
    {
        if (!EntitiesCollection.SanctuarySides.ContainsKey(slug))
        {
            return;
        }

        var entity = GetEntity(transferStruct);
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

    public IEntity GetEntity(IPersistanceTransferStruct transferable)
    {
        SanctuarySideDTO? nullableSanctuarySideDTO = transferable as SanctuarySideDTO?;

        if (nullableSanctuarySideDTO is null)
        {
            return null;
        }

        var sanctuarySideDTO = nullableSanctuarySideDTO.Value;
        
        Church sanctuarySideChurch = 
            (
                EntitiesCollection.Churches.ContainsKey(sanctuarySideDTO.ChurchSlug)
                ) ? EntitiesCollection.Churches[sanctuarySideDTO.ChurchSlug] : null;

        SanctuarySide sanctuarySide = new SanctuarySide
        {
            Name = sanctuarySideDTO.Name,
            Slug = sanctuarySideDTO.Slug,
            Regions = null,
            Church = sanctuarySideChurch,
            ChurchSlug = sanctuarySideDTO.ChurchSlug
        };

        if (sanctuarySideDTO.Regions != null)
        {
            sanctuarySide.Regions =
                sanctuarySideDTO.Regions.Select(
                    e => GetEntity(e) as SanctuaryRegion
                    ).ToList();
        }

        if (sanctuarySideChurch != null) {
            sanctuarySideChurch.SanctuarySides.Add(sanctuarySide);
        }

        return sanctuarySide;
    }
}