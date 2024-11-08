using Moq;
using StainedGlass.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Tests.Transfer;

public class UseCaseInteractorTest
{
    [Fact]
    public void UseCaseInteractor()
    {
        InputBoundary useCaseInteractor = new UseCaseInteractor();
        var mockSanctuaryRegion = new Mock<SanctuaryRegionDTO>();
        StainedGlassDTO stainedGlassDTO = new StainedGlassDTO
        {
            Title = "StainedGlass1",
            Slug = "stainedGlass1",
            Description = "description",
            Image = "image",
            SanctuaryRegionDTO = mockSanctuaryRegion.Object,
        };
    }
}