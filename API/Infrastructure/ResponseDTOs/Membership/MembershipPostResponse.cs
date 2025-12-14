namespace API.Infrastructure.ResponseDTOs.Membership;

public class MembershipPostResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
}
