using API.Infrastructure.RequestDTOs.Shared;

namespace API.Infrastructure.RequestDTOs.Membership;

public class MembershipGetRequest : BaseFilterModel
{
    public MembershipGetFilter Filter { get; set; }
}
