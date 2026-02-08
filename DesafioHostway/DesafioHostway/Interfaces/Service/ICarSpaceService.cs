using DesafioHostway.Models.Request;
using DesafioHostway.Models.Response;
using DesafioHostway.Models.Response.Wrappers;

namespace DesafioHostway.Interfaces.Service;

public interface ICarSpaceService
{
    Task<ServiceResponse<IEnumerable<CarSpaceResponse>>> GetAllSpacesByParkingLots(Guid parkingLotId);
    Task<ServiceResponse<CarSpaceResponse>> GetSpaceById(Guid spaceId);
    Task<ServiceResponse<CarSpaceResponse>> CreateNewCarSpace(CarSpaceRequest request);
    Task<ServiceResponse<CarSpaceResponse>> UpdateCarSpace(Guid CarSpaceId, CarSpaceRequest request);
    Task<ServiceResponse<bool>> DeleteSpace(Guid spaceId, Guid parkingLotId);
}
