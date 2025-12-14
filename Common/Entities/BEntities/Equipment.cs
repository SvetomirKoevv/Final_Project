namespace Common.Entities.BEntities;

public class Equipment : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int QunatityAvailable { get; set; }

    public List<TrainingSession> TrainingSessions { get; set; }
}