namespace API.Infrastructure.RequestDTOs. TrainingSession;

public class TrainingSessionRequest
{
    public int CoachId { get; set; }
    public int HallId { get; set; }
    public int SportId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int MaxParticipants { get; set; }
}
