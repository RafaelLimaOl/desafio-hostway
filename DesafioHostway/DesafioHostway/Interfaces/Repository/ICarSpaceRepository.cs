using DesafioHostway.Models.Request;
using DesafioHostway.Models.Response;

namespace DesafioHostway.Interfaces.Repository;

public interface ICarSpaceRepository
{
    Task<IEnumerable<CarSpaceResponse>> GetAllCarsSpaces(Guid userId);
    Task<IEnumerable<CarSpaceResponse>> GetAllSpacesByParkingLot(Guid parkingId);
    Task<CarSpaceResponse?> GetCarSpaceById(Guid spaceId);
    Task<CarSpaceResponse> CreateCarSpace(CarSpaceRequest request);
    Task<bool> UpdateCarSpace(Guid spaceId, CarSpaceRequest request);
    Task<bool> DeleteCarSpace(Guid spaceId);
    Task<bool> ExistSpaceById(Guid spaceId);
    Task<bool> CarSpaceContainsInParkingLot(Guid spaceId, Guid parkingId);
    Task<bool> CurrentSpaceIsAvaliable(Guid spaceId);
    Task<bool> UserCanUseAccessibleParkingSpace(Guid spaceId, Guid userId);
}
