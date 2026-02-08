namespace DesafioHostway.Models.Request;

public class CarSpaceRequest
{
    public int SpaceNumber { get; set; }
    public bool IsAccessibleParkingSpace { get; set; }
    public Guid ParkingLotId { get; set; }
    public bool IsActive { get; set; }
}
