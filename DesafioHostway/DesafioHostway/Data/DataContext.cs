using DesafioHostway.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DesafioHostway.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    public DbSet<Car> Cars { get; set; }
    public DbSet<CarSpace> CarSpaces { get; set; }
    public DbSet<ParkingLot> ParkingLots { get; set; }
    public DbSet<ParkingSession> ParkingSessions { get; set; }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var idProperty = entity.FindProperty("Id");

            if (idProperty != null && idProperty.ClrType == typeof(Guid))
            {
                idProperty.ValueGenerated = Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.OnAdd;
            }
        }


        modelBuilder.Entity<ParkingLot>()
            .HasMany(p => p.CarSpaces)
            .WithOne(c => c.ParkingLot)
            .HasForeignKey(c => c.ParkingLotId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
