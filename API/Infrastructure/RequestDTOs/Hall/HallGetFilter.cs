namespace API.Infrastructure.RequestDTOs.Hall;

public class HallGetFilter
{
    public string Name { get; set; }
    public int MinCapacity { get; set; }
    public int MaxCapacity { get; set; }
    public int Floor { get; set; }
}
