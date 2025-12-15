namespace API.Infrastructure.ResponseDTOs.User;

public class UserPostResponse
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int? MembershipId { get; set; }
}
