using DesafioHostway.Models.Request;
using DesafioHostway.Models.Response;
using DesafioHostway.Models.Response.Wrappers;

namespace DesafioHostway.Interfaces.Service;

public interface ICarService
{
    Task<ServiceResponse<IEnumerable<CarResponse>>> GetAllCars(Guid userId);
    Task<ServiceResponse<CarResponse>> GetCarById(Guid carId);
    Task<ServiceResponse<CarResponse>> CreateCar(CarRequest request);
    Task<ServiceResponse<CarResponse>> UpdateCar(Guid CarId, CarRequest request);
    Task<ServiceResponse<bool>> DeleteCar(Guid UserId, Guid CarId);
}
