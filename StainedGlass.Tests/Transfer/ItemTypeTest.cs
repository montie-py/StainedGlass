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
        Assert.Equal(2, dtos.Count());
    }
    
}