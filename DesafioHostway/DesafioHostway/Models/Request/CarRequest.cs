namespace DesafioHostway.Models.Request;

public class CarRequest
{
    public string LicensePlate { get; set; }
    public string Color { get; set; }
    public string Model { get; set; }

    public Guid UserId { get; set; }
    public bool IsActive { get; set; }
}