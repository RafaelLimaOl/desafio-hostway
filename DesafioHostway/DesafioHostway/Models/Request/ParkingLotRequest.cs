namespace DesafioHostway.Models.Request;

public class ParkingLotRequest
{
    public string Name { get; set; }
    public string Address { get; set; }
    public decimal FirstHourAmount { get; set; }
    public decimal ExtraHourAmount { get; set; }
    public Guid UserId{ get; set; }
    public bool IsActive  { get; set; }   
}
