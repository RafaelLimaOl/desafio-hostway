using DesafioHostway.Models.Request;
using DesafioHostway.Models.Response;

namespace DesafioHostway.Interfaces.Repository;

public interface ICarRepository
{
    Task<IEnumerable<CarResponse>> GetAllCars(Guid userId);
    Task<CarResponse?> GetCarById(Guid carId);
    Task<CarResponse> CreateCar(CarRequest request);
    Task<bool> UpdateCar(Guid carId, CarRequest request);
    Task<bool> DeleteCar(Guid carId);
    Task<bool> ExistCarById(Guid carId);
    Task<bool> CarContainsInUser(Guid carId, Guid userId);
    Task<bool> CarExistsByLicensePlate(Guid? carId, string licensePlate);
}
