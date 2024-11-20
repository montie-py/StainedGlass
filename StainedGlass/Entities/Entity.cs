namespace StainedGlass.Entities;

public interface Entity
{
     public string Slug {get; set;}
    public void Save();
    public void Replace(string slug, Entity entity);
    public void Remove(string slug);

}