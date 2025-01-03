using StainedGlass.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Tests.Transfer;

public class ItemTest
{
    InputBoundary useCaseInteractor;

    public ItemTest()
    {
        useCaseInteractor = new UseCaseInteractor();
    }
    
    [Fact]
    public async void SanctuaryRegionNullRelatedItemsNull()
    {
        var stainedGlassDTO = new ItemDTO
        {
            Title = "StainedGlass8",
            Slug = "StainedGlassSlug8",
            Description = "StainedGlass Description",
            Image = "StainedGlass Image",
        };

        useCaseInteractor.StoreEntity(stainedGlassDTO);
        ItemDTO savedItemDto = await useCaseInteractor.GetDTOBySlug<ItemDTO>(stainedGlassDTO.Slug);
        
        Assert.Equal(stainedGlassDTO, savedItemDto);
    }
    
    [Fact]
    public async void RelatedItemsSanctuaryRegionNull()
    {
        var relatedStainedGlassDTO = new ItemDTO
        {
            Title = "StainedGlassRelated",
            Slug = "StainedGlassRelatedSlug",
            Description = "StainedGlassRelated Description",
            Image = "StainedGlassRelated Image",
            SanctuaryRegion = null,
            SanctuaryRegionSlug = null,
            RelatedItemsSlugs = null,
            RelatedItems = null,
        };
        
        useCaseInteractor.StoreEntity(relatedStainedGlassDTO);
        
        var stainedGlassDTO = new ItemDTO
        {
            Title = "StainedGlass",
            Slug = "StainedGlassSlug",
            Description = "StainedGlass Description",
            Image = "StainedGlass Image",
            SanctuaryRegion = null,
            SanctuaryRegionSlug = null,
            RelatedItemsSlugs = new HashSet<string>(){relatedStainedGlassDTO.Slug},
            RelatedItems = null,
        };

        useCaseInteractor.StoreEntity(stainedGlassDTO);
        ItemDTO savedItemDto = await useCaseInteractor.GetDTOBySlug<ItemDTO>(stainedGlassDTO.Slug);
        
        Assert.Equal(relatedStainedGlassDTO, savedItemDto.RelatedItems.First().Value);
    }

    [Fact]
    public async void ItemsShouldBeLikewiseRelated()
    {
        var item1 = new ItemDTO
        {
            Title = "StainedGlass5",
            Slug = "StainedGlassSlug5",
            Description = "StainedGlass5Description",
            Image = "StainedGlass Image",
        };
        useCaseInteractor.StoreEntity(item1);

        var item2 = new ItemDTO
        {
            Title = "StainedGlass6",
            Slug = "StainedGlassSlug6",
            Description = "StainedGlass6Description",
            Image = "StainedGlass Image",
            RelatedItemsSlugs = new HashSet<string>(){item1.Slug}
        };
        useCaseInteractor.StoreEntity(item2);
        
        var item1FromDB = await useCaseInteractor.GetDTOBySlug<ItemDTO>(item1.Slug);
        var item2FromDB = await useCaseInteractor.GetDTOBySlug<ItemDTO>(item2.Slug);
        
        Assert.Equal(item1, item2FromDB.RelatedItems.First().Value);
        Assert.Equal(item2, item1FromDB.RelatedItems.First().Value);
    }

    [Fact]
    public async void ItemRegionNotNull()
    {
        var sanctuaryRegion = new SanctuaryRegionDTO
        {
            Name = "Region",
            Slug = "RegionSlug",
            Description = "Region Description",
            // Image = "Region Image",
        };
        
        useCaseInteractor.StoreEntity(sanctuaryRegion);

        var item = new ItemDTO
        {
            Title = "StainedGlass9",
            Slug = "StainedGlassSlug9",
            Description = "StainedGlass Description",
            Image = "StainedGlass Image",
            SanctuaryRegionSlug = "RegionSlug",
        };
        
        useCaseInteractor.StoreEntity(item);
        var savedItem = await useCaseInteractor.GetDTOBySlug<ItemDTO>(item.Slug);
        Assert.Equal(sanctuaryRegion, savedItem.SanctuaryRegion);
    }
    
    [Fact]
    public async void ItemReplace()
    {
        var item = new ItemDTO
        {
            Title = "StainedGlass2",
            Slug = "stainedGlassSlug2",
            Description = "StainedGlass Description",
            Image = "StainedGlass Image",
        };
        
        useCaseInteractor.StoreEntity(item);

        var newItem = new ItemDTO
        {
            Title = "StainedGlass3",
            Slug = "stainedGlassSlug3",
            Description = "StainedGlass Description",
            Image = "StainedGlass Image",
        };
        
        useCaseInteractor.ReplaceEntity("stainedGlassSlug2", newItem);
        var dto = await useCaseInteractor.GetDTOBySlug<ItemDTO>(item.Slug);
        Assert.Equal("StainedGlass3", dto.Title);
        
        //todo: check replacing with null and not null region and itemtype
    }

    [Fact]
    public async void ItemDelete()
    {
        var region = new SanctuaryRegionDTO
        {
            Name = "Region",
            Slug = "RegionSlug",
            Description = "Region Description",
            // Image = "Region Image",
        };
        
        useCaseInteractor.StoreEntity(region);
        
        var item = new ItemDTO
        {
            Title = "StainedGlass4",
            Slug = "stainedGlassSlug4",
            Description = "StainedGlass Description",
            Image = "StainedGlass Image",
            SanctuaryRegionSlug = "RegionSlug",
        };
        useCaseInteractor.StoreEntity(item);
        var storedRegion = await useCaseInteractor.GetDTOBySlug<SanctuaryRegionDTO>(region.Slug);
        Assert.True(storedRegion.Items.Any(e => e.Slug == "stainedGlassSlug4"));
        
        useCaseInteractor.RemoveEntity<ItemDTO>(item.Slug);
        var dtos = await useCaseInteractor.GetAllDTOs<ItemDTO>() as List<ItemDTO>;
        
        Assert.False(dtos.Exists(e => e.Title == "StainedGlass4"));
        storedRegion = await useCaseInteractor.GetDTOBySlug<SanctuaryRegionDTO>(region.Slug);
        Assert.False(storedRegion.Items.Any(e => e.Slug == "stainedGlassSlug4"));
    }

    [Fact]
    public async void ItemWithItemTypeShouldBeStored()
    {
        var itemType = new ItemTypeDTO
        {
            Name = "ItemType",
            Slug = "ItemTypeSlug",
        };
        useCaseInteractor.StoreEntity(itemType);
        var item = new ItemDTO
        {
            Title = "StainedGlass8",
            Slug = "StainedGlassSlug8",
            Description = "StainedGlass Description",
            Image = "StainedGlass Image",
            ItemTypeSlug = itemType.Slug,
        };
        useCaseInteractor.StoreEntity(item);
        var storedItem = await useCaseInteractor.GetDTOBySlug<ItemDTO>(item.Slug);
        
        Assert.Equal(itemType.Slug, storedItem.ItemType.Slug);
    }
}