using StainedGlass.Entities;
using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Transfer.DTOs;

public class StainedGlassDTO : Transferable
{
    public string Slug {get; set;}
    public string Title{get; set;}
    public string Description{get; set;}
    public string Image{get; set;}
    public string? SanctuaryRegionSlug {get; set;}
    public SanctuaryRegionDTO? SanctuaryRegion {get; set;}


    public Entity GetEntity(Transferable transferable)
    {
        return (new StainedGlassMapper()).GetEntity(transferable);
    }

    public Mappable GetMapper()
    {
        return new StainedGlassMapper();
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType()) 
        { return false; }
        
        var other = (StainedGlassDTO)obj;

        return Slug == other.Slug
               && Title == other.Title
               && Description == other.Description
               && Image == other.Image;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Slug, Title, Description, Image);
    }
}