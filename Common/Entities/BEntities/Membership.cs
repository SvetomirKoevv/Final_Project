namespace Common.Entities.BEntities;

public class Membership : BaseEntity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
}
