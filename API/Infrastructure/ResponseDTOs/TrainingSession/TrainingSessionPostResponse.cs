namespace API.Infrastructure.ResponseDTOs.TrainingSessionP;

public class TrainingSessionPostResponse
{
    public int Id { get; set; }
    public int SportId { get; set; }
    public int CoachId { get; set; }
    public int HallId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int MaxParticipants { get; set; }
}
