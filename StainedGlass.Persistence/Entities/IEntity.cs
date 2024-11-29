namespace StainedGlass.Persistence.Entities;

public interface IEntity
{
    public string Slug {get; set;}
    public void Save();
    public void Replace(string slug, IEntity entity);
    public void Remove();

}