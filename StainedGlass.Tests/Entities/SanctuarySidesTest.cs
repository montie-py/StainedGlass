namespace StainedGlass.Tests.Entities;

using Moq;
using StainedGlass.Entities;

public class SanctuarySidesTest
{
    private readonly SanctuarySide sanctuarySide;

    public SanctuarySidesTest()
    {
        var mockStainedGlass1 = new Mock<StainedGlass>();
        var mockStainedGlass2 = new Mock<StainedGlass>();
        var mockStainedGlass3 = new Mock<StainedGlass>();
        var mockStainedGlass4 = new Mock<StainedGlass>();

        var mockChurch = new Mock<Church>();

        sanctuarySide = new SanctuarySide
        {
            Slug = "slug",
            Name = "name",
            Regions = null,
            Church = mockChurch.Object,
        };

        SanctuaryRegion sanctuaryRegion1 = new SanctuaryRegion
        {
            Slug = "slug1",
            Name = "name1",
            Image = "image1",
            Windows = new HashSet<StainedGlass>(){mockStainedGlass1.Object, mockStainedGlass2.Object},
            SanctuarySide = sanctuarySide,
        };

        SanctuaryRegion sanctuaryRegion2 = new SanctuaryRegion
        {
            Slug = "slug2",
            Name = "name2",
            Image = "image2",
            Windows = new HashSet<StainedGlass>(){mockStainedGlass3.Object, mockStainedGlass4.Object},
            SanctuarySide = sanctuarySide,
        };

        sanctuarySide.Regions = new List<SanctuaryRegion>(){sanctuaryRegion1, sanctuaryRegion2};
    }

    [Fact]
    public void SanctuarySides_MainPropertiesShouldBeSetCorrectly()
    {
        Assert.Equal("slug", sanctuarySide.Slug);
        Assert.Equal("name", sanctuarySide.Name);
    }

    [Fact]
    public void SanctuarySides_RegionsPropertyShouldHaveExactNumberOfObjects()
    {
        Assert.Equal(2, sanctuarySide.Regions.Count);
    }

    [Fact]
    public void SanctuarySides_RegionsShouldBeSetCorrectly()
    {
        Assert.Equal("slug1", sanctuarySide.Regions[0].Slug);
        Assert.Equal("slug2", sanctuarySide.Regions[1].Slug);
    }
}