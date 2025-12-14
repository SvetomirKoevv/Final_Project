namespace API.Infrastructure.RequestDTOs.Membership;

public class MembershipRequest
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
}
