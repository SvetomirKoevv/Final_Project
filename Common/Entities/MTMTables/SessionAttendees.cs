using Common.Entities.BEntities;

namespace Common.Entities.MTMTables;

public class SessionAttendees
{
    public int UserId { get; set; }
    public int TrainingSessionId { get; set; }

    public virtual User User { get; set; }
    public virtual TrainingSession TrainingSession { get; set; }
}
