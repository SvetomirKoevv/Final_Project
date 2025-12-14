using API.Infrastructure.RequestDTOs.Shared;

namespace API.Infrastructure.RequestDTOs. TrainingSession;

public class TrainingSessionGetRequest : BaseFilterModel
{
    public TrainingsSessionGetFIlter Filter { get; set; }
}
