namespace API.Infrastructure.RequestDTOs.Equipment;

public class EquipmentGetFilter
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int MinQuantity { get; set; }
    public int MaxQuantity { get; set; }
}
