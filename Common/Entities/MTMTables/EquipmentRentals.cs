using Common.Entities.BEntities;

namespace Common.Entities.MTMTables;

public class EquipmentRentals
{
    public int EquipmentId { get; set; }
    public int TrainingSessionId { get; set; }
    public int Quantity { get; set; }

    public virtual Equipment Equipment { get; set; }
    public virtual TrainingSession TrainingSession { get; set; }
}
