namespace API.Infrastructure.RequestDTOs.Membership;

public class MembershipGetFilter
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int MinurationDays { get; set; }
    public int MaxnurationDays { get; set; }
}
