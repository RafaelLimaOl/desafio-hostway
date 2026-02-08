using Microsoft.EntityFrameworkCore;

namespace DesafioHostway.Models.Entities;

[Index(nameof(CarId), nameof(ExitTime))]
public class ParkingSession
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CarId { get; set; }
    public Guid SpaceId { get; set; }

    public DateTime EntryTime { get; set; }
    public DateTime? ExitTime { get; set; }

    public decimal? AmountChanged { get; set; }
    public bool IsClosed => ExitTime != null;
    public TimeSpan? TotalTime { get; set; }

    public required Car Car { get; set; }
    public required CarSpace Space { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
