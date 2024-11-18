using StainedGlass.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Tests.Transfer;

public class SanctuaryRegionTest
{
    InputBoundary useCaseInteractor;
    
    public SanctuaryRegionTest()
    {
        useCaseInteractor = new UseCaseInteractor();
    }
    
    [Fact]
    public void SanctuaryRegion_PropertiesShouldBeSetCorrectly()
    {
        var stainedGlassDTO = new ItemDTO
        {
            Title = "StainedGlass",
            Slug = "StainedGlassSlug",
            Description = "StainedGlass Description",
            Image = "StainedGlass Image",
            SanctuaryRegion = null,
            SanctuaryRegionSlug = null
        };

        var sanctuaryRegionDTO = new SanctuaryRegionDTO
        {
            Name = "SanctuaryRegion",
            Slug = "SanctuaryRegionSlug",
            Image = "SanctuaryRegion Image",
            Windows = new HashSet<ItemDTO>(){stainedGlassDTO},
            SanctuarySide = null,
            SanctuarySideSlug = null
        };
        
        useCaseInteractor.StoreEntity(sanctuaryRegionDTO);
        SanctuaryRegionDTO savedSanctuaryRegionDto = useCaseInteractor.GetDTOBySlug<SanctuaryRegionDTO>("SanctuaryRegionSlug");
        
        Assert.Equal(stainedGlassDTO, savedSanctuaryRegionDto.Windows.First());
    }
    
    //TODO write tests with nullable Windows and non-nullable Sides
}