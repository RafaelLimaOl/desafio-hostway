namespace DesafioHostway.Models.Response;

public class ParkingSessionResponse
{
    public Guid Id {  get; set; }
    public Guid CarId { get; set; }
    public Guid SpaceId {  get; set; }
    public DateTime EntryTime {  get; set; }
    public DateTime ExitTime {  get; set; }
    public Decimal AmountChanged { get; set; }
    public string TotalTime {  get; set; }

    public bool IsActive { get; set; }
}
