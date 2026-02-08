namespace DesafioHostway.Models.Request;

public class ParkinSessionCreateRequest
{
    public Guid CarId { get; set; }
    public Guid SpaceId { get; set; }
    public DateTime EntryTime { get; set; }
}
