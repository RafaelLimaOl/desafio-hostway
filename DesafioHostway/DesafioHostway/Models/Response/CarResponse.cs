namespace DesafioHostway.Models.Response;

public class CarResponse
{
    public Guid Id { get; set; }
    public string LicensePlate { get; set; }
    public string Color { get; set; }
    public string Model { get; set; }

    public bool? IsActive { get; set; }
    public Guid UserId { get; set; }
}

