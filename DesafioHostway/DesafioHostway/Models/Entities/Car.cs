using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesafioHostway.Models.Entities;

[Index(nameof(LicensePlate), IsUnique = true)]
public class Car
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required, MaxLength(8)]
    public string LicensePlate { get; set; } = null!;
    [MaxLength(50)]
    public string? Color { get; set; }

    [MaxLength(100)]
    public string Model { get; set; } = null!;

    public Guid UserId { get; set; } = Guid.NewGuid();
    public required User User { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}