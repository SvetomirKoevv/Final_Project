using API.Infrastructure.RequestDTOs.Shared;

namespace API.Infrastructure.RequestDTOs.Role;

public class RoleGetRequest : BaseFilterModel
{
    public RoleGetFilter Filter { get; set; }
}
