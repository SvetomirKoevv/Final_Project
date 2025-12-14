using API.Infrastructure.RequestDTOs.Shared;

namespace API.Infrastructure.RequestDTOs.Hall;

public class HallGetRequest : BaseFilterModel
{
    public HallGetFilter Filter { get; set; }
}
