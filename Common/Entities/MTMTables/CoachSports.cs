using Common.Entities.BEntities;

namespace Common.Entities.MTMTables;

public class CoachSports
{
    public int CoachId { get; set; }
    public int SportId { get; set; }

    public virtual Coach Coach { get; set; }
    public virtual Sport Sport { get; set; }
}
