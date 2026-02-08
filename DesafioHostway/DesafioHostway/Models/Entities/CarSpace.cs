using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesafioHostway.Models.Entities;

[Index(nameof(SpaceNumber), IsUnique = true)]
public class CarSpace
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; } = Guid.NewGuid();
    public int SpaceNumber { get; set; }

    public Guid ParkingLotId { get; set; }
    public ParkingLot ParkingLot { get; set; }

    public bool IsAccessibleParkingSpace { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
