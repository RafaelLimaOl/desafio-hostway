using DesafioHostway.Models.Request;
using DesafioHostway.Models.Response;

namespace DesafioHostway.Interfaces.Repository;

public interface IParkingLotRepository
{
    Task<IEnumerable<ParkingLotResponse>> GetAllParkingLots(Guid userId);
    Task<ParkingLotResponse?> GetParkingLotById(Guid parkingId);
    Task<IEnumerable<ParkingLotResponse>> GetAllParkingLotAvaliable();
    Task<ParkingLotResponse> CreateNewParkingLot(ParkingLotRequest request);
    Task<ParkingLotWithCarSpacesResponse> GetParkingLotWithCarSpaces(Guid parkingLotId);
    Task<bool> UpdateParkingLot(Guid parkingId, ParkingLotRequest request);
    Task<bool> DeleteParking(Guid parkingId);
    Task<int> CountSpacesInParkingById(Guid parkingId, bool searchInactive);
    Task<bool> ExistParkingById(Guid parkingId);
    Task<bool> ParkingContainsInUser(Guid parkingId, Guid userId);
}
