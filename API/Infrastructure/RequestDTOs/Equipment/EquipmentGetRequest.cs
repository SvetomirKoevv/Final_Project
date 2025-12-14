using API.Infrastructure.RequestDTOs.Shared;

namespace API.Infrastructure.RequestDTOs.Equipment;

public class EquipmentGetRequest : BaseFilterModel
{
    public EquipmentGetFilter Filter { get; set; }
}
