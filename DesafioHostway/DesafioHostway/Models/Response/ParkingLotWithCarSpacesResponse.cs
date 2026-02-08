namespace DesafioHostway.Models.Response;

public class ParkingLotWithCarSpacesResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public decimal FirstHourAmount { get; set; }
    public decimal ExtraHourAmount { get; set; }
    public Guid UserId { get; set; }
    public bool IsActive { get; set; }

    public List<CarSpaceWithParkingLot> CarSpaces { get; set; } = new();
}

public class CarSpaceWithParkingLot
{
    public Guid CarSpaceId { get; set; }
    public int SpaceNumber { get; set; }
    public Guid ParkingLotId { get; set; }
    public bool IsAccessibleParkingSpace { get; set; }
    public bool IsActive { get; set; }
}