namespace StainedGlass.Tests.Entities;

using Moq;
using StainedGlass.Entities;
using System.Linq;

public class SanctuaryRegionsTest
{
    private readonly SanctuaryRegion sanctuaryRegion;
    public SanctuaryRegionsTest()
    {
        var mockSanctuarySide = new Mock<SanctuarySide>();

        sanctuaryRegion = new SanctuaryRegion()
        {
            Slug = "slug",
            Name = "name",
            Image = "image",
            Windows = null,
            SanctuarySide = mockSanctuarySide.Object,
        };

        StainedGlass window1 = new StainedGlass
        {
            Slug = "slug1",
            Title = "title1",
            Description = "description1",
            Image = "image1",
            SanctuaryRegion = sanctuaryRegion
        };

        StainedGlass window2 = new StainedGlass
        {
            Slug = "slug2",
            Title = "title2",
            Description = "description2",
            Image = "image2",
            SanctuaryRegion = sanctuaryRegion
        };

        sanctuaryRegion.Windows = new HashSet<StainedGlass>(){window1, window2};
    }

   [Fact]
    public void SanctuaryRegions_PropertiesShouldBeSetCorrectly()
    {
        Assert.Equal("slug", sanctuaryRegion.Slug);
        Assert.Equal("name", sanctuaryRegion.Name);
        Assert.Equal("image", sanctuaryRegion.Image);
    }

    [Fact]
    public void SanctuaryRegions_WindowsPropertyShouldHaveExactNumberOfObjects()
    {
        Assert.Equal(2, sanctuaryRegion.Windows.Count);
    }

    [Fact]
    public void SanctuaryRegions_WindowsPropertyHasObjectsWithParticularPropertyValues()
    {
        Assert.True(sanctuaryRegion.Windows.Any(window => window.Title == "title2"));
    }
}