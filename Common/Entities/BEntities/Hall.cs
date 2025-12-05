using System.Data;

namespace Common.Entities.BEntities;

public class Hall : BaseEntity
{
    public string Name { get; set; }
    public int Capacity { get; set; }
    public int Floor { get; set; }
}
