namespace Common.Entities.BEntities;

public class TrainingSession : BaseEntity
{
    public int SportId { get; set; }
    public int CoachId { get; set; }
    public int HallId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int MaxParticipants { get; set; }
}
