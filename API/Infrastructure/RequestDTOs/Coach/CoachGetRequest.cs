using API.Infrastructure.RequestDTOs.Shared;

namespace API.Infrastructure.RequestDTOs.Coach;

public class CoachGetRequest : BaseFilterModel
{
    public CoachGetFilter Filter { get; set; }
}
