namespace StainedGlass.Tests.Entities;

using StainedGlass.Entities;
using System.Linq;

public class SanctuaryRegionsTest
{
    private readonly SanctuaryRegion sanctuaryRegions;
    public SanctuaryRegionsTest()
    {
        StainedGlass window1 = new StainedGlass
        {
            Slug = "slug1",
            Title = "title1",
            Description = "description1",
            Image = "image1"
        };

        StainedGlass window2 = new StainedGlass
        {
            Slug = "slug2",
            Title = "title2",
            Description = "description2",
            Image = "image2"
        };

        sanctuaryRegions = new SanctuaryRegion()
        {
            Slug = "slug",
            Name = "name",
            Image = "image",
            Windows = new HashSet<StainedGlass>(){window1, window2}
        };
    }

   [Fact]
    public void SanctuaryRegions_PropertiesShouldBeSetCorrectly()
    {
        Assert.Equal("slug", sanctuaryRegions.Slug);
        Assert.Equal("name", sanctuaryRegions.Name);
        Assert.Equal("image", sanctuaryRegions.Image);
    }

    [Fact]
    public void SanctuaryRegions_WindowsPropertyShouldHaveExactNumberOfObjects()
    {
        Assert.Equal(2, sanctuaryRegions.Windows.Count);
    }

    [Fact]
    public void SanctuaryRegions_WindowsPropertyHasObjectsWithParticularPropertyValues()
    {
        Assert.True(sanctuaryRegions.Windows.Any(window => window.Title == "title2"));
    }
}