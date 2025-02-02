﻿using StainedGlass.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Tests.Transfer;

public class SanctuaryRegionTest
{
    InputBoundary useCaseInteractor;
    
    public SanctuaryRegionTest()
    {
        useCaseInteractor = new UseCaseInteractor();
    }

    [Fact]
    public async void AddSanctuaryRegionWithNoSide()
    {
        var sanctuaryRegion = new SanctuaryRegionDTO
        {
            Name = "SanctuaryRegion6",
            Slug = "SanctuaryRegionSlug6",
            // Image = "SanctuaryRegion Image",
            Description = "SanctuaryRegion Description",
        };
        useCaseInteractor.StoreEntity(sanctuaryRegion);
        var savedSanctuaryRegion = await useCaseInteractor.GetDTOBySlug<SanctuaryRegionDTO>(sanctuaryRegion.Slug);
        Assert.Equal(sanctuaryRegion, savedSanctuaryRegion);
    }
    
    [Fact]
    public async void AddSanctuaryRegionWithASide()
    {
        var sanctuarySide = new SanctuarySideDTO
        {
            Name = "SanctuarySide",
            Slug = "SanctuarySideSlug",
        };
        useCaseInteractor.StoreEntity(sanctuarySide);
        var sanctuaryRegion = new SanctuaryRegionDTO
        {
            Name = "SanctuaryRegion1",
            Slug = "SanctuaryRegionSlug1",
            // Image = "SanctuaryRegion Image1",
            SanctuarySideSlug = "SanctuarySideSlug"
        };
        useCaseInteractor.StoreEntity(sanctuaryRegion);
        var savedSanctuaryRegion = await useCaseInteractor.GetDTOBySlug<SanctuaryRegionDTO>("SanctuaryRegionSlug1");
        var savedSanctuarySide = await useCaseInteractor.GetDTOBySlug<SanctuarySideDTO>("SanctuarySideSlug");
        Assert.Equal(savedSanctuaryRegion, savedSanctuarySide.Regions.First());
    }

    [Fact]
    public async void GetItemsByRegion()
    {
        var region = new SanctuaryRegionDTO
        {
            Name = "SanctuaryRegion3",
            Slug = "SanctuaryRegionSlug3",
            Description = "SanctuaryRegion Description",
            // Image = "SanctuaryRegion Image",
        };
        
        useCaseInteractor.StoreEntity(region);

        var item1 = new ItemDTO
        {
            Title = "Item 1",
            Slug = "ItemSlug1",
            Description = "Item Description",
            Image = "Item Image 1",
            SanctuaryRegionSlug = region.Slug
        };

        var item2 = new ItemDTO
        {
            Title = "Item 2",
            Slug = "ItemSlug2",
            Description = "Item Description",
            Image = "Item Image 2",
            SanctuaryRegionSlug = region.Slug
        };
        
        useCaseInteractor.StoreEntity(item1);
        useCaseInteractor.StoreEntity(item2);

        var savedRegion = await useCaseInteractor.GetDTOBySlug<SanctuaryRegionDTO>(region.Slug);
        Assert.Equal(2, savedRegion.Items.Count());
        Assert.All(savedRegion.Items, item => Assert.Equal("Item Description", item.Description));
    }

    [Fact]
    public async void ReplaceRegion()
    {
        var sanctuaryRegion = new SanctuaryRegionDTO
        {
            Name = "SanctuaryRegion4",
            Slug = "SanctuaryRegionSlug4",
            Description = "SanctuaryRegion Description4",
            // Image = "SanctuaryRegion Image",
        };
        
        useCaseInteractor.StoreEntity(sanctuaryRegion);

        var anotherSanctuaryRegion = new SanctuaryRegionDTO
        {
            Name = "SanctuaryRegion5",
            Slug = "SanctuaryRegionSlug5",
            Description = "SanctuaryRegion Description5",
            // Image = "SanctuaryRegion Image",
        };
        
        useCaseInteractor.ReplaceEntity(sanctuaryRegion.Slug, anotherSanctuaryRegion);
        var replacedRegion = await useCaseInteractor.GetDTOBySlug<SanctuaryRegionDTO>(sanctuaryRegion.Slug);
        Assert.Equal(anotherSanctuaryRegion.Name, replacedRegion.Name);
        
        //todo: check replacing with null and not null side
    }

    [Fact]
    public async void RemoveRegion()
    {
        var sanctuarySide = new SanctuarySideDTO
        {
            Name = "SanctuarySide",
            Slug = "SanctuarySideSlug",
        };
        useCaseInteractor.StoreEntity(sanctuarySide);
        
        var sanctuaryRegion = new SanctuaryRegionDTO
        {
            Name = "SanctuaryRegion7",
            Slug = "SanctuaryRegionSlug7",
            Description = "SanctuaryRegion Description7",
            // Image = "SanctuaryRegion Image",
            SanctuarySideSlug = sanctuarySide.Slug
        };
        useCaseInteractor.StoreEntity(sanctuaryRegion);
        var storedSide = await useCaseInteractor.GetDTOBySlug<SanctuarySideDTO>(sanctuarySide.Slug);
        Assert.True(storedSide.Regions.Any(e => e.Slug == "SanctuaryRegionSlug7"));
        
        useCaseInteractor.RemoveEntity<SanctuaryRegionDTO>(sanctuaryRegion.Slug);
        var allRegions = await useCaseInteractor.GetAllDTOs<SanctuaryRegionDTO>() as List<SanctuaryRegionDTO>;
        
        Assert.False(allRegions.Exists(x => x.Slug == sanctuaryRegion.Slug));
        storedSide = await useCaseInteractor.GetDTOBySlug<SanctuarySideDTO>(sanctuarySide.Slug);
        Assert.False(storedSide.Regions.Any(e => e.Slug == "SanctuaryRegionSlug7"));
    }
}