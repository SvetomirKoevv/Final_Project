using API.Infrastructure.RequestDTOs.Shared;

namespace API.Infrastructure.RequestDTOs.Sport;

public class SportGetRequest : BaseFilterModel
{
    public SportGetFilter Filter { get; set; }
}
