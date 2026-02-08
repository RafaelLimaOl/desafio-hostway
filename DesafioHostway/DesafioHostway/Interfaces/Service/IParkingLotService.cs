using DesafioHostway.Models.Request;
using DesafioHostway.Models.Response;
using DesafioHostway.Models.Response.Wrappers;

namespace DesafioHostway.Interfaces.Service;

public interface IParkingLotService
{
    Task<ServiceResponse<IEnumerable<ParkingLotResponse>>> GetAllParkingLots(Guid userId);
    Task<ServiceResponse<ParkingLotResponse>> GetParkingLotById(Guid parkingLotId);
    Task<ServiceResponse<IEnumerable<ParkingLotResponse>>> GetAllAvaliableParkingLots();
    Task<ServiceResponse<ParkingLotWithCarSpacesResponse>> GetParkingLotAndCarSpaces(Guid parkingLotId);
    Task<ServiceResponse<ParkingLotResponse>> CreateParkingLot(ParkingLotRequest request);
    Task<ServiceResponse<ParkingLotResponse>> UpdateParkingLot(Guid ParkingLotId, ParkingLotRequest request);
    Task<ServiceResponse<bool>> DeleteParkingLot(Guid parkingLotId, Guid userId);
}
