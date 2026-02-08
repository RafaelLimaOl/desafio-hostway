namespace DesafioHostway.Models.Request;

public class ParkingSessionRequest
{
    public Guid CarId { get; set; }
    public Guid SpaceId { get; set; }
    public DateTime EntryTime { get; set; }
    public DateTime ExitTime { get; set; } 
}
