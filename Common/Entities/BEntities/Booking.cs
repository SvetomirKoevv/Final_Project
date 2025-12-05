namespace Common.Entities.BEntities;

public class Booking : BaseEntity
{
    public int UserId { get; set; }
    public int TrainingSessionId { get; set; }
}
