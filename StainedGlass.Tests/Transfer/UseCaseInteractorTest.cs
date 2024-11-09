using StainedGlass.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Tests.Transfer;

public class UseCaseInteractorTest
{
    InputBoundary useCaseInteractor;

    public UseCaseInteractorTest()
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
            SanctuaryRegionSlug = ""
        };

        useCaseInteractor.StoreEntity(stainedGlassDTO);
    }
}