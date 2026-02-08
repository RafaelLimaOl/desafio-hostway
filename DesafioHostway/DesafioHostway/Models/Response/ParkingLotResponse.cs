using DesafioHostway.Models.Entities;

namespace DesafioHostway.Models.Response;

public class ParkingLotResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public decimal FirstHourAmount { get; set; }
    public decimal ExtraHourAmount { get; set; }
    public int LotsAvaliable { get; set; }
    public Guid UserId { get; set; }
    public bool IsActive { get; set; }

    public ICollection<CarSpace> CarSpaces { get; set; } = [];
}
