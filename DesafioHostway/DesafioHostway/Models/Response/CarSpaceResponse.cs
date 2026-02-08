namespace DesafioHostway.Models.Response;

public class CarSpaceResponse
{
    public Guid Id {  get; set; }
    public int SpaceNumber { get; set; }
    public Guid ParkingLotId { get; set; }
    public bool IsAccessibleParkingSpace { get; set; }
    public bool IsActive { get; set; }
}
