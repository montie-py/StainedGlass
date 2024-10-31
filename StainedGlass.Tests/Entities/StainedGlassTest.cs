namespace StainedGlass.Tests.Entities;
using StainedGlass.Entities;

public class StainedGlassTest
{
    [Fact]
    public void StainedGlass_PropertiesShouldBeSetCorrectly()
    {
        StainedGlass stainedGlass = new StainedGlass
        {
            Slug = "slug",
            Title = "title",
            Description = "description",
            Image = "image"

        };
        Assert.Equal("slug", stainedGlass.Slug);
        Assert.Equal("title", stainedGlass.Title);
        Assert.Equal("description", stainedGlass.Description);
        Assert.Equal("image", stainedGlass.Image);
        
    }
}