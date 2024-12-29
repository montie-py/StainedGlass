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
    public async void TestChurchCreation()
    {
        try
        {
            var churchDTO = new ChurchDTO
            {
                Name = "Church",
                Slug = "church_slug",
                Description = "Description",
                // Image = "Image",
            };
            useCaseInteractor.StoreEntity(churchDTO);
        
            var storedEntity = await useCaseInteractor.GetDTOBySlug<ChurchDTO>(churchDTO.Slug);
            Assert.Equal(churchDTO, storedEntity);
        }
        catch (Exception e)
        {
            throw; // TODO handle exception
        }
    }

    [Fact]
    public async void GetSidesByChurch()
    {
        var church = new ChurchDTO
        {
            Name = "Church 2",
            Slug = "slug-2",
            Description = "Description",
            // Image = "Image",
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

        var savedChurch = await useCaseInteractor.GetDTOBySlug<ChurchDTO>(church.Slug);
        
        Assert.Equal(2, savedChurch.Sides.Count);
        Assert.True(new[]{side1.Slug, side2.Slug}.Contains(savedChurch.Sides.First().Slug));
        Assert.True(new[]{side1.Slug, side2.Slug}.Contains(savedChurch.Sides.Last().Slug));
    }

    [Fact]
    public async void ReplaceChurch()
    {
        var church = new ChurchDTO
        {
            Name = "Church 3",
            Slug = "slug-3",
            Description = "Description",
            // Image = "Image",
        };
        useCaseInteractor.StoreEntity(church);

        var dtoToReplace = new ChurchDTO
        {
            Name = "Church 4",
            Slug = "slug-4",
            Description = "Church 4 Description",
            // Image = "Church 4 Image",
        };
        
        useCaseInteractor.ReplaceEntity(church.Slug, dtoToReplace);
        var replacedChurch = await useCaseInteractor.GetDTOBySlug<ChurchDTO>(church.Slug);
        
        Assert.Equal(dtoToReplace.Name, replacedChurch.Name);
        Assert.Equal(dtoToReplace.Description, replacedChurch.Description);
        Assert.Equal(dtoToReplace.Image, replacedChurch.Image);
    }

    [Fact]
    public async void RemoveChurch()
    {
        var church = new ChurchDTO
        {
            Name = "Church 5",
            Slug = "slug-5",
            Description = "Description",
            // Image = "Image",
        };
        
        useCaseInteractor.StoreEntity(church);
        useCaseInteractor.RemoveEntity<ChurchDTO>(church.Slug);
        var churches = await useCaseInteractor.GetAllDTOs<ChurchDTO>() as List<ChurchDTO>;
        
        Assert.False(churches.Exists(c => c.Slug == church.Slug));
    }
}