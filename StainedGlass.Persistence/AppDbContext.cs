using Microsoft.EntityFrameworkCore;
using StainedGlass.Persistence.Entities;

namespace StainedGlass.Persistence;

internal class AppDbContext : DbContext
{
    internal DbSet<Church> Churches { get; set; }
    internal DbSet<SanctuarySide> SanctuarySides { get; set; }
    internal DbSet<SanctuaryRegion> SanctuaryRegions { get; set; }
    internal DbSet<Item> Items { get; set; }
    internal DbSet<ItemType> ItemTypes { get; set; }
    internal DbSet<ItemRelation> ItemRelations { get; set; }
    internal DbSet<ItemImage> ItemImages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var relativePath = @"Data\StainedGlass.db";
        var absolutePath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).FullName, relativePath);
        optionsBuilder.UseSqlite($"Data Source={absolutePath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //indicating the right table name for this entity
        modelBuilder.Entity<ItemImage>().ToTable("ItemImages");
        
        // church-sanctuaryside relationship
        modelBuilder.Entity<Church>()
            .HasMany(church => church.SanctuarySides)
            .WithOne(sanctuarySide => sanctuarySide.Church)
            .HasForeignKey(sanctuarySide => sanctuarySide.ChurchSlug);
        
        //sanctuaryside-sanctuaryregion relationship
        modelBuilder.Entity<SanctuarySide>()
            .HasMany(sanctuarySide => sanctuarySide.Regions)
            .WithOne(region => region.SanctuarySide)
            .HasForeignKey(sanctuaryRegion => sanctuaryRegion.SanctuarySideSlug);
        
        //sanctuaryregion-item relationship
        modelBuilder.Entity<SanctuaryRegion>()
            .HasMany(sanctuaryRegion => sanctuaryRegion.Items)
            .WithOne(item => item.SanctuaryRegion)
            .HasForeignKey(item => item.SanctuaryRegionSlug);
        
        //itemtype-item relationship
        modelBuilder.Entity<ItemType>()
            .HasMany(itemType => itemType.Items)
            .WithOne(item => item.ItemType)
            .HasForeignKey(item => item.ItemTypeSlug)
            .OnDelete(DeleteBehavior.SetNull);

        //item-relateditems relationship
        modelBuilder.Entity<ItemRelation>()
            .HasKey(itemRelation => new { itemRelation.ItemSlug, itemRelation.RelatedItemSlug });
        
        modelBuilder.Entity<Item>() 
            .HasMany(i => i.RelatedItems) 
            .WithOne(ir => ir.Item) 
            .HasForeignKey(ir => ir.ItemSlug);
        
        modelBuilder.Entity<ItemRelation>() 
            .HasOne(ir => ir.RelatedItem) 
            .WithMany() 
            .HasForeignKey(ir => ir.RelatedItemSlug);
        
        //item-itemimages relationship
        modelBuilder.Entity<Item>()
            .HasMany(item => item.ItemImages)
            .WithOne(itemImage => itemImage.Item)
            .HasForeignKey(itemImage => itemImage.ItemSlug);
        
        // base.OnModelCreating(modelBuilder);
    }
}