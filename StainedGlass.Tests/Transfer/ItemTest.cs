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
    public void StainedGlass_SanctuaryRegionNullRelatedItemsNull()
    {
        var stainedGlassDTO = new ItemDTO
        {
            Title = "StainedGlass",
            Slug = "StainedGlassSlug",
            Description = "StainedGlass Description",
            Image = "StainedGlass Image",
            SanctuaryRegion = null,
            SanctuaryRegionSlug = null,
            RelatedItemsSlugs = null,
            RelatedItems = null,
        };

        useCaseInteractor.StoreEntity(stainedGlassDTO);
        ItemDTO savedItemDto = useCaseInteractor.GetDTOBySlug<ItemDTO>("StainedGlassSlug");
        
        Assert.Equal(stainedGlassDTO, savedItemDto);
    }
    
    public void StainedGlass_SanctuaryRegionNull()
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
            RelatedItemsSlugs = new HashSet<string>(){"StainedGlassRelatedSlug"},
            RelatedItems = null,
        };

        useCaseInteractor.StoreEntity(stainedGlassDTO);
        ItemDTO savedItemDto = useCaseInteractor.GetDTOBySlug<ItemDTO>("StainedGlassSlug");
        
        Assert.Equal(stainedGlassDTO.RelatedItems.First(), relatedStainedGlassDTO);
    }
}