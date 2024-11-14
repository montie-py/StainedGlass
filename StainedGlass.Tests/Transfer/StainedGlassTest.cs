using StainedGlass.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Tests.Transfer;

public class StainedGlassTest
{
    InputBoundary useCaseInteractor;

    public StainedGlassTest()
    {
        useCaseInteractor = new UseCaseInteractor();
    }
    
    [Fact]
    public void StainedGlass_PropertiesShouldBeSetCorrectly()
    {
        var stainedGlassDTO = new StainedGlassDTO
        {
            Title = "StainedGlass",
            Slug = "StainedGlassSlug",
            Description = "StainedGlass Description",
            Image = "StainedGlass Image",
            SanctuaryRegion = null,
            SanctuaryRegionSlug = null
        };

        useCaseInteractor.StoreEntity(stainedGlassDTO);
        StainedGlassDTO savedStainedGlassDTO = useCaseInteractor.GetDTOBySlug<StainedGlassDTO>("StainedGlassSlug");
        
        Assert.Equal(stainedGlassDTO, savedStainedGlassDTO);
    }
}