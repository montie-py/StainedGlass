using Moq;
using StainedGlass.Entities;

namespace StainedGlass.Tests.Entities;

public class ChurchesTest
{
    private readonly Churches churches;

    public ChurchesTest()
    {
        var mockSanctuaryRegion1 = new Mock<SanctuaryRegion>();
        var mockSanctuaryRegion2 = new Mock<SanctuaryRegion>();
        var mockSanctuaryRegion3 = new Mock<SanctuaryRegion>();
        var mockSanctuaryRegion4 = new Mock<SanctuaryRegion>();

        var sanctuarySide1 = new SanctuarySide
        {
            Slug = "slug1",
            Name = "name1",
            Regions = new List<SanctuaryRegion>(){mockSanctuaryRegion1.Object, mockSanctuaryRegion2.Object}
        };
        var sanctuarySide2 = new SanctuarySide
        {
            Slug = "slug2",
            Name = "name2",
            Regions = new List<SanctuaryRegion>(){mockSanctuaryRegion3.Object, mockSanctuaryRegion4.Object}
        };

        churches = new Churches
        {
            Slug = "slug",
            Name = "slug",
            Image = "image",
            Sides = new HashSet<SanctuarySide>(){sanctuarySide1, sanctuarySide2}
        };
    }

    [Fact]
    public void Churches_PropertiesShouldBeSetCorrectly()
    {
        Assert.Equal("slug", churches.Slug);
        Assert.Equal("name", churches.Name);
        Assert.Equal("image", churches.Image);
    }

    public void Churches_RegionsPropertyShouldBeSetCorrectly()
    {
        Assert.True(churches.Sides.Any(side => side.Slug == "slug2"));
    }
}