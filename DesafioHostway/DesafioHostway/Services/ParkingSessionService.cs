using DesafioHostway.Interfaces.Repository;
using DesafioHostway.Interfaces.Service;
using DesafioHostway.Models.Request;
using DesafioHostway.Models.Response;
using DesafioHostway.Models.Response.Wrappers;
using static System.Collections.Specialized.BitVector32;

namespace DesafioHostway.Services;

public class ParkingSessionService(
    IParkingSessionRepository parkingSessionRepository, 
    ICarRepository carRepository, 
    IParkingLotRepository parkingLotRepository,
    ICarSpaceRepository carSpaceRepository,
    IUserAccountRepository userAccountRepository
    ) : IParkingSessionService
{
    private readonly IParkingSessionRepository _parkingSessionRepository = parkingSessionRepository;
    private readonly ICarRepository _carRepository = carRepository;
    private readonly IParkingLotRepository _parkingLotRepository = parkingLotRepository;
    private readonly ICarSpaceRepository _carSpaceRepository = carSpaceRepository;
    private readonly IUserAccountRepository _userAccountRepository = userAccountRepository;
    public async Task<ServiceResponse<ParkingSessionResponse>> GetSessionByCar(Guid carId)
    {
        if (carId == Guid.Empty)
            return ServiceResponse<ParkingSessionResponse>.Fail("Id inválido");

        var existsCar = await _carRepository.ExistCarById(carId);

        if (!existsCar)
            return ServiceResponse<ParkingSessionResponse>.Fail("Carro não encontrado");

        var result = await _parkingSessionRepository.GetAllSessionByCar(carId);

        if (result == null)
            return ServiceResponse<ParkingSessionResponse>.Ok("Sem dados");

        return ServiceResponse<ParkingSessionResponse>.Ok(result);
    }

    public async Task<ServiceResponse<IEnumerable<ParkingSessionResponse>>> GetSessionByUserId(Guid userId)
    {
        if (userId == Guid.Empty)
            return ServiceResponse<IEnumerable<ParkingSessionResponse>>.Fail("Id inválido");

        var existsUser = await _userAccountRepository.UserExistById(userId);

        if (!existsUser)
            return ServiceResponse<IEnumerable<ParkingSessionResponse>>.Fail("Usuário não encontrado");

        var result = await _parkingSessionRepository.GetAllSessionsByUser(userId);

        if (result == null || !result.Any())
            return ServiceResponse<IEnumerable<ParkingSessionResponse>>.Ok("Sem dados");

        return ServiceResponse<IEnumerable<ParkingSessionResponse>>.Ok(result);
    }
    public async Task<ServiceResponse<ParkingSessionResponse>> GetSessionBySpace(Guid spaceId)
    {
        if (spaceId == Guid.Empty)
            return ServiceResponse<ParkingSessionResponse>.Fail("Id inválido");

        var existsCar = await _parkingLotRepository.ExistParkingById(spaceId);

        if (!existsCar)
            return ServiceResponse<ParkingSessionResponse>.Fail("Carro não encontrado");

        var result = await _parkingSessionRepository.GetAllSessionBySpaceId(spaceId);

        if (result == null)
            return ServiceResponse<ParkingSessionResponse>.Ok("Sem dados");

        return ServiceResponse<ParkingSessionResponse>.Ok(result);
    }

    public async Task<ServiceResponse<ParkingSessionResponse>> GetSessionById(Guid sessionId)
    {
        if (sessionId == Guid.Empty)
            return ServiceResponse<ParkingSessionResponse>.Fail("Id inválido");

        var result = await _parkingSessionRepository.GetSessionById(sessionId);

        var possibleExitTime = DateTime.UtcNow;
        var totalTime = possibleExitTime - result.EntryTime;

        var price = await _parkingSessionRepository.GetParkingPrice(sessionId);
        if (price == null)
            return ServiceResponse<ParkingSessionResponse>.Fail("Sessão não encontrada");

        var totalHours = totalTime.TotalHours;

        decimal amount;

        if (totalHours <= 1)
        {
            amount = price.FirstHourAmount;
        }
        else
        {
            var extraHours = Math.Ceiling(totalHours - 1);

            amount = price.FirstHourAmount +
                     ((decimal)extraHours * price.ExtraHourAmount);
        }

        result.AmountChanged = amount;
        result.TotalTime = totalTime.ToString(@"hh\:mm\:ss");

        if (result == null)
            return ServiceResponse<ParkingSessionResponse>.Fail("Sessão não encontrada");

        return ServiceResponse<ParkingSessionResponse>.Ok(result);
    }
    
    public async Task<ServiceResponse<ParkingSessionResponse>> OpenNewSession(Guid userId, ParkinSessionCreateRequest request)
    {
        var carExists = await _carRepository.ExistCarById(request.CarId);
        if (!carExists)
            return ServiceResponse<ParkingSessionResponse>.Fail("Carro inválido");

        var spaceExists = await _carSpaceRepository.ExistSpaceById(request.SpaceId);
        if (!spaceExists)
            return ServiceResponse<ParkingSessionResponse>.Fail("Vaga inválida");

        var hasOpenSession = await _parkingSessionRepository.HasOpenSession(request.CarId);
        if (hasOpenSession)
            return ServiceResponse<ParkingSessionResponse>.Fail("Carro já possui sessão aberta");

        var space = await _carSpaceRepository.GetCarSpaceById(request.SpaceId);
        var user = await _userAccountRepository.GetUserInfo(userId);

        if (space.IsAccessibleParkingSpace && !user.HaveDeficiency)
            return ServiceResponse<ParkingSessionResponse>.Fail("Vaga exclusiva para pessoas com deficiência");

        var result = await _parkingSessionRepository.CreateNewSession(request);

        return ServiceResponse<ParkingSessionResponse>.Ok(result);
    }
    
    public async Task<ServiceResponse<ParkingSessionResponse>> CloseCurrentSession(Guid sessionId)
    {
        if (sessionId == Guid.Empty)
            return ServiceResponse<ParkingSessionResponse>.Fail("Id inválido");

        var session = await _parkingSessionRepository.GetSessionById(sessionId);

        if (session == null)
            return ServiceResponse<ParkingSessionResponse>.Fail("Sessão não encontrada");

        if (session.ExitTime != DateTime.MinValue)
            return ServiceResponse<ParkingSessionResponse>.Fail("Sessão já encerrada");

        var exitTime = DateTime.UtcNow;
        var totalTime = exitTime - session.EntryTime;

        var price = await _parkingSessionRepository.GetParkingPrice(sessionId);

        if (price == null)
            return ServiceResponse<ParkingSessionResponse>.Fail("Sessão não encontrada");

        var totalHours = totalTime.TotalHours;

        decimal amount;

        if (totalHours <= 1)
        {
            amount = price.FirstHourAmount;
        }
        else
        {
            var extraHours = Math.Ceiling(totalHours - 1);

            amount = price.FirstHourAmount +
                     ((decimal)extraHours * price.ExtraHourAmount);
        }

        await _parkingSessionRepository.CloseCurrentSession(sessionId, exitTime, amount, totalTime);

        var updatedSession = await _parkingSessionRepository.GetSessionById(sessionId);

        return ServiceResponse<ParkingSessionResponse>.Ok(updatedSession!);
    }

    public async Task<ServiceResponse<bool>> DeleteSession(Guid sessionId)
    {
        if (sessionId == Guid.Empty)
            return ServiceResponse<bool>.Fail("Id inválido");

        var existsSessionById = await _parkingSessionRepository.ExistSessionById(sessionId);

        if (!existsSessionById)
            return ServiceResponse<bool>.Fail("Sessão não encontrada");

        var result = await _parkingSessionRepository.DeleteSession(sessionId);

        if (!result)
            return ServiceResponse<bool>.Fail("Erro ao deletar");

        return ServiceResponse<bool>.Ok("Sessão deletada com sucesso");
    }

}
