using Moq;
using StainedGlass.Entities;

namespace StainedGlass.Tests.Entities;

public class ChurchesTest
{
    private readonly Church church;

    public ChurchesTest()
    {
        var mockSanctuaryRegion1 = new Mock<SanctuaryRegion>();
        var mockSanctuaryRegion2 = new Mock<SanctuaryRegion>();
        var mockSanctuaryRegion3 = new Mock<SanctuaryRegion>();
        var mockSanctuaryRegion4 = new Mock<SanctuaryRegion>();

        church = new Church
        {
            Slug = "slug",
            Name = "name",
            Image = "image",
            Sides = null,
        };

        var sanctuarySide1 = new SanctuarySide
        {
            Slug = "slug1",
            Name = "name1",
            Regions = new List<SanctuaryRegion>(){mockSanctuaryRegion1.Object, mockSanctuaryRegion2.Object},
            Church = church,
        };
        
        var sanctuarySide2 = new SanctuarySide
        {
            Slug = "slug2",
            Name = "name2",
            Regions = new List<SanctuaryRegion>(){mockSanctuaryRegion3.Object, mockSanctuaryRegion4.Object},
            Church = church
        };

        church.Sides = new HashSet<SanctuarySide>(){sanctuarySide1, sanctuarySide2};
    }

    [Fact]
    public void Churches_PropertiesShouldBeSetCorrectly()
    {
        Assert.Equal("slug", church.Slug);
        Assert.Equal("name", church.Name);
        Assert.Equal("image", church.Image);
    }

    public void Churches_RegionsPropertyShouldBeSetCorrectly()
    {
        Assert.True(church.Sides.Any(side => side.Slug == "slug2"));
    }
}