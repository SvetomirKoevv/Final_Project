using API.Infrastructure.RequestDTOs.Shared;

namespace API.Infrastructure.RequestDTOs.User;

public class UserGetRequest : BaseFilterModel
{
    public UserGetFilter Filter { get; set; }
}
