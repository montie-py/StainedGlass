using StainedGlass.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Tests.Transfer;

public class ItemTypeTest
{
    private ItemTypeDTO _itemTypeDto;

    private InputBoundary useCaseInteractor;
    
    public ItemTypeTest()
    {
        useCaseInteractor = new UseCaseInteractor();
    }

    [Fact]
    public void SaveOne()
    {
        _itemTypeDto = new ItemTypeDTO
        {
            Name = "Test",
            Slug = "test"
        };
        useCaseInteractor.StoreEntity(_itemTypeDto);
        var dtos = useCaseInteractor.GetAllDTOs<ItemTypeDTO>() as List<ItemTypeDTO>;
        Assert.True(dtos.Exists(x => x.Name == "Test"));
    }

    [Fact]
    public void SaveMany()
    {
        useCaseInteractor.StoreEntity(new ItemTypeDTO
        {
            Name = "Test1",
            Slug = "test1"
        });
        useCaseInteractor.StoreEntity(new ItemTypeDTO
        {
            Name = "Test2",
            Slug = "test2"
        });
        IEnumerable<ItemTypeDTO> dtos = useCaseInteractor.GetAllDTOs<ItemTypeDTO>();
        Assert.Equal(2, dtos.Where(e => new[] { "Test1", "Test2" }.Contains(e.Name)).Count());
    }

    [Fact]
    public void Remove()
    {
        useCaseInteractor.StoreEntity(new ItemTypeDTO
        {
            Name = "Test3",
            Slug = "test3"
        });
        useCaseInteractor.RemoveEntity<ItemTypeDTO>("test3");
        var dtos = useCaseInteractor.GetAllDTOs<ItemTypeDTO>() as List<ItemTypeDTO>;
        Assert.False(dtos.Exists(x => x.Name == "Test3"));
    }

    [Fact]
    public void Replace()
    {
        useCaseInteractor.StoreEntity(new ItemTypeDTO
        {
            Name = "Test4",
            Slug = "test4"
        });

        var newItemTypeDTO = new ItemTypeDTO
        {
            Name = "Test5",
            Slug = "test5"
        };
        useCaseInteractor.ReplaceEntity("test4", newItemTypeDTO);
        var dto = useCaseInteractor.GetDTOBySlug<ItemTypeDTO>("test4");
        Assert.Equal("Test5", dto.Name);
    }
}