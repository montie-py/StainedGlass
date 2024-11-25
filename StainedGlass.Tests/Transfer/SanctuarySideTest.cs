using StainedGlass.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Tests.Transfer;

public class SanctuarySideTest
{
    InputBoundary useCaseInteractor;

    public SanctuarySideTest()
    {
        useCaseInteractor = new UseCaseInteractor();
    }

    [Fact]
    public void TestSideWithNoChurch()
    {
        var side = new SanctuarySideDTO
        {
            Name = "Sanctuary Side",
            Slug = "sanctuary-side",
        };
        
        useCaseInteractor.StoreEntity(side);
        Assert.NotNull(useCaseInteractor.GetDTOBySlug<SanctuarySideDTO>(side.Slug));
    }

    [Fact]
    public void TestSideWithChurch()
    {
        var church = new ChurchDTO
        {
            Name = "Church",
            Slug = "church",
            Description = "Church description",
            Image = "Church image"
        };
        
        useCaseInteractor.StoreEntity(church);
        var side = new SanctuarySideDTO
        {
            Name = "Sanctuary Side1",
            Slug = "sanctuary-side1",
            ChurchSlug = church.Slug
        };
        useCaseInteractor.StoreEntity(side);
        var savedChurch = useCaseInteractor.GetDTOBySlug<ChurchDTO>(church.Slug);
        Assert.Equal(side, savedChurch.Sides?.First() );
    }

    [Fact]
    public void GetRegionsBySide()
    {
        var side = new SanctuarySideDTO
        {
            Name = "Sanctuary Side3",
            Slug = "sanctuary-side3",
        };
        useCaseInteractor.StoreEntity(side);
        var region1 = new SanctuaryRegionDTO
        {
            Name = "Region 1",
            Slug = "sanctuary-region-1",
            Image = "sanctuary-region-1",
            Description = "Region description",
            SanctuarySideSlug = side.Slug
        };
        useCaseInteractor.StoreEntity(region1);
        var region2 = new SanctuaryRegionDTO
        {
            Name = "Region 2",
            Slug = "sanctuary-region-2",
            Image = "sanctuary-region-2",
            Description = "Region description",
            SanctuarySideSlug = side.Slug
        };
        useCaseInteractor.StoreEntity(region2);
        
        var savedSide = useCaseInteractor.GetDTOBySlug<SanctuarySideDTO>(side.Slug);
        Assert.Equal(2, savedSide.Regions.Count);
        Assert.All(savedSide.Regions, region => Assert.Equal("Region description", region.Description));
    }

    [Fact]
    public void ReplaceSide()
    {
        var oldside = new SanctuarySideDTO
        {
            Name = "Sanctuary Side 4",
            Slug = "sanctuary-side-4"
        };
        var newside = new SanctuarySideDTO
        {
            Name = "Sanctuary Side 5",
            Slug = "sanctuary-side-5",
        };
        useCaseInteractor.StoreEntity(oldside);
        useCaseInteractor.ReplaceEntity(oldside.Slug, newside);

        var replacedSide = useCaseInteractor.GetDTOBySlug<SanctuarySideDTO>(oldside.Slug);
        Assert.Equal(newside.Name, replacedSide.Name);
    }

    [Fact]
    public void RemoveSide()
    {
        var side = new SanctuarySideDTO
        {
            Name = "Sanctuary Side 6",
            Slug = "sanctuary-side-6"
        };
        useCaseInteractor.StoreEntity(side);
        useCaseInteractor.RemoveEntity<SanctuarySideDTO>(side.Slug);

        var allSides = useCaseInteractor.GetAllDTOs<SanctuarySideDTO>() as List<SanctuarySideDTO>;
        Assert.False(allSides.Exists(s => s.Slug == side.Slug));
        
        //todo: check if church doesn't have the deleted side anymore
    }
}