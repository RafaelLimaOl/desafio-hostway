using DesafioHostway.Models.Request;
using DesafioHostway.Models.Response;

namespace DesafioHostway.Interfaces.Repository;

public interface IParkingSessionRepository
{
    Task<ParkingSessionResponse> GetAllSessionByCar(Guid carId);
    Task<ParkingSessionResponse> GetAllSessionBySpaceId(Guid spaceId);
    Task<IEnumerable<ParkingSessionResponse>> GetAllSessionsByUser(Guid userId);
    Task<ParkingSessionResponse?> GetSessionById(Guid id);
    Task<ParkingSessionResponse> CreateNewSession(ParkinSessionCreateRequest request);
    Task<bool> CloseCurrentSession(Guid sessionId, DateTime exitTime, decimal amount, TimeSpan totalTime);
    Task<bool> DeleteSession(Guid id);

    Task<ParkingPriceResponse> GetParkingPrice(Guid sessionId);
    Task<bool> ExistSessionById(Guid id);
    Task<bool> HasOpenSession(Guid carId);
}
