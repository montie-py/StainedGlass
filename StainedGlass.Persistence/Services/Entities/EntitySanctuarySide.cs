using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services.Entities;

public class EntitySanctuarySide : INonRelatable, IPersistenceService
{
    public async Task<bool> AddEntity(IPersistanceTransferStruct transferStruct)
    {
        var sanctuarySideDto = (SanctuarySideDTO)transferStruct;
        var entity = GetEntity(sanctuarySideDto) as SanctuarySide;
        EntitiesCollection.SanctuarySides.TryAdd(sanctuarySideDto.Slug, entity);
        return true;
    } 
    
    public async Task<ICollection<IPersistanceTransferStruct>> GetAllDtos()
    {
        return EntitiesCollection.SanctuarySides.Select(
            e => GetDTOForEntity(e.Value, skipParentElements: true)
        )
        .ToList();
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

    public async Task<IPersistanceTransferStruct?> GetDtoBySlug(string slug)
    {
        if (!EntitiesCollection.SanctuarySides.ContainsKey(slug))
        {
            return null;
        }
        return GetDTOForEntity(EntitiesCollection.SanctuarySides[slug], skipParentElements: true);
    }

    public async Task<bool> RemoveEntity(string slug)
    {
        if (!EntitiesCollection.SanctuarySides.ContainsKey(slug))
        {
           return false;  
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
        return true;
    }

    public async Task<bool> ReplaceEntity(string slug, IPersistanceTransferStruct transferStruct)
    {
        if (!EntitiesCollection.SanctuarySides.ContainsKey(slug))
        {
            return false;
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
        return true;
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
            Position = sanctuarySideDTO.Position,
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