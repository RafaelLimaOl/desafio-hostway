using DesafioHostway.Models.Request;
using DesafioHostway.Models.Response;
using DesafioHostway.Models.Response.Wrappers;

namespace DesafioHostway.Interfaces.Service;

public interface IParkingSessionService
{
    Task<ServiceResponse<ParkingSessionResponse>> GetSessionByCar(Guid carId);
    Task<ServiceResponse<ParkingSessionResponse>> GetSessionBySpace(Guid spaceId);
    Task<ServiceResponse<IEnumerable<ParkingSessionResponse>>> GetSessionByUserId(Guid userId);
    Task<ServiceResponse<ParkingSessionResponse>> GetSessionById(Guid sessionId);
    Task<ServiceResponse<ParkingSessionResponse>> OpenNewSession(Guid userId, ParkinSessionCreateRequest request);
    Task<ServiceResponse<ParkingSessionResponse>> CloseCurrentSession(Guid sessionId);
    Task<ServiceResponse<bool>> DeleteSession(Guid sessionId);
}
