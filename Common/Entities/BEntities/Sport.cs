namespace Common.Entities.BEntities;

public class Sport : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }

    public virtual List<Coach> Coaches { get; set; }
}
