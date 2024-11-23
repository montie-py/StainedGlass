using StainedGlass.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Tests.Transfer;

public class ChurchTest
{
    InputBoundary useCaseInteractor;
    public ChurchTest()
    {
        useCaseInteractor = new UseCaseInteractor();
    }

    [Fact]
    public void TestChurchCreation()
    {
        var churchDTO = new ChurchDTO
        {
            Name = "Church",
            Slug = "church_slug",
            Description = "Description",
            Image = "Image",
        };
        useCaseInteractor.StoreEntity(churchDTO);
        
        var storedEntity = useCaseInteractor.GetDTOBySlug<ChurchDTO>(churchDTO.Slug);
        Assert.Equal(churchDTO, storedEntity);
    }

    [Fact]
    public void GetSidesByChurch()
    {
        var church = new ChurchDTO
        {
            Name = "Church 2",
            Slug = "slug-2",
            Description = "Description",
            Image = "Image",
        };
        useCaseInteractor.StoreEntity(church);

        var side1 = new SanctuarySideDTO
        {   
            Name = "Side 1",
            Slug = "slug-1",
            ChurchSlug = church.Slug,
        };
        var side2 = new SanctuarySideDTO
        {
            Name = "Side 2",
            Slug = "slug-2",
            ChurchSlug = church.Slug
        };
        
        useCaseInteractor.StoreEntity(side1);
        useCaseInteractor.StoreEntity(side2);

        var savedChurch = useCaseInteractor.GetDTOBySlug<ChurchDTO>(church.Slug);
        
        Assert.Equal(2, savedChurch.Sides.Count);
        Assert.True(new[]{side1.Slug, side2.Slug}.Contains(savedChurch.Sides.First().Slug));
        Assert.True(new[]{side1.Slug, side2.Slug}.Contains(savedChurch.Sides.Last().Slug));
    }

    [Fact]
    public void ReplaceChurch()
    {
        var church = new ChurchDTO
        {
            Name = "Church 3",
            Slug = "slug-3",
            Description = "Description",
            Image = "Image",
        };
        useCaseInteractor.StoreEntity(church);

        var dtoToReplace = new ChurchDTO
        {
            Name = "Church 4",
            Slug = "slug-4",
            Description = "Church 4 Description",
            Image = "Church 4 Image",
        };
        
        useCaseInteractor.ReplaceEntity(church.Slug, dtoToReplace);
        var replacedChurch = useCaseInteractor.GetDTOBySlug<ChurchDTO>(church.Slug);
        
        Assert.Equal(dtoToReplace.Name, replacedChurch.Name);
        Assert.Equal(dtoToReplace.Description, replacedChurch.Description);
        Assert.Equal(dtoToReplace.Image, replacedChurch.Image);
    }

    [Fact]
    public void RemoveChurch()
    {
        var church = new ChurchDTO
        {
            Name = "Church 5",
            Slug = "slug-5",
            Description = "Description",
            Image = "Image",
        };
        
        useCaseInteractor.StoreEntity(church);
        useCaseInteractor.RemoveEntity<ChurchDTO>(church.Slug);
        var churches = useCaseInteractor.GetAllDTOs<ChurchDTO>() as List<ChurchDTO>;
        
        Assert.False(churches.Exists(c => c.Slug == church.Slug));
    }
}