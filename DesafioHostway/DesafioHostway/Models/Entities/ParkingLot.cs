using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesafioHostway.Models.Entities;

public class ParkingLot
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string Address { get; set; }
    public decimal FirstHourAmount { get; set; }
    public decimal ExtraHourAmount { get; set; }

    public Guid UserId {  get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public ICollection<CarSpace> CarSpaces { get; set; } = [];
}
