namespace API.Infrastructure.RequestDTOs. TrainingSession;

public class TrainingsSessionGetFIlter
{
    public int? SportId { get; set; }
    public int? CoachId { get; set; }
    public int? HallId { get; set; }
    public DateTime? FromStartTime { get; set; }
    public DateTime? ToStartTime { get; set; }
    public DateTime? FromEndTime { get; set; }
    public DateTime? ToEndTime { get; set; }
    public int? MaxParticipants { get; set; }
}
