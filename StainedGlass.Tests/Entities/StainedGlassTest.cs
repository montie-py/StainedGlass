namespace StainedGlass.Tests.Entities;

using Moq;
using StainedGlass.Entities;

public class StainedGlassTest
{
    [Fact]
    public void StainedGlass_PropertiesShouldBeSetCorrectly()
    {
        var mockSanctuaryRegion = new Mock<SanctuaryRegion>();
        
        StainedGlass stainedGlass = new StainedGlass
        {
            Slug = "slug",
            Title = "title",
            Description = "description",
            Image = "image",
            SanctuaryRegion = mockSanctuaryRegion.Object,
        };
        Assert.Equal("slug", stainedGlass.Slug);
        Assert.Equal("title", stainedGlass.Title);
        Assert.Equal("description", stainedGlass.Description);
        Assert.Equal("image", stainedGlass.Image);
        
    }
}