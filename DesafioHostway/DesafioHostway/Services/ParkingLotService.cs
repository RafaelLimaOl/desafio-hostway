using DesafioHostway.Interfaces.Repository;
using DesafioHostway.Interfaces.Service;
using DesafioHostway.Models.Entities;
using DesafioHostway.Models.Request;
using DesafioHostway.Models.Response;
using DesafioHostway.Models.Response.Wrappers;

namespace DesafioHostway.Services;

public class ParkingLotService(IParkingLotRepository parkingLotRepository, IUserAccountRepository userAccountRepository) : IParkingLotService
{
    private readonly IParkingLotRepository _parkingLotRepository = parkingLotRepository;
    private readonly IUserAccountRepository _userAccountRepository = userAccountRepository;

    public async Task<ServiceResponse<IEnumerable<ParkingLotResponse>>> GetAllParkingLots(Guid userId)
    {
        if (userId == Guid.Empty)
            return ServiceResponse<IEnumerable<ParkingLotResponse>>.Fail("Id Inválido");

        var result = await _parkingLotRepository.GetAllParkingLots(userId);

        if (result == null || !result.Any())
            return ServiceResponse<IEnumerable<ParkingLotResponse>>.Ok("Sem Data");

        return ServiceResponse<IEnumerable<ParkingLotResponse>>.Ok(result);
    }

    public async Task<ServiceResponse<ParkingLotResponse>> GetParkingLotById(Guid parkingLotId)
    {
        if (parkingLotId == Guid.Empty)
            return ServiceResponse<ParkingLotResponse>.Fail("Id Inválido");
        var existParkingLot = await _parkingLotRepository.GetParkingLotById(parkingLotId);
        var countCarSpaces = await _parkingLotRepository.CountSpacesInParkingById(parkingLotId, true);

        existParkingLot.LotsAvaliable = countCarSpaces;

        if (existParkingLot == null)
            return ServiceResponse<ParkingLotResponse>.Fail($"Estacionamento não encontrado com o Id fornecido: {parkingLotId}");

        return ServiceResponse<ParkingLotResponse>.Ok(existParkingLot);
    }

    public async Task<ServiceResponse<IEnumerable<ParkingLotResponse>>> GetAllAvaliableParkingLots()
    {
        var result = await _parkingLotRepository.GetAllParkingLotAvaliable();
        return ServiceResponse<IEnumerable<ParkingLotResponse>>.Ok(result);
    }

    public async Task<ServiceResponse<ParkingLotWithCarSpacesResponse>> GetParkingLotAndCarSpaces(Guid parkingLotId)
    {
        if (parkingLotId == Guid.Empty)
            return ServiceResponse<ParkingLotWithCarSpacesResponse>.Fail("Id Inválido");
        var existParkingLot = await _parkingLotRepository.ExistParkingById(parkingLotId);

        if (!existParkingLot)
            return ServiceResponse<ParkingLotWithCarSpacesResponse>.Fail($"Estacionamento não encontrado com o Id fornecido: {parkingLotId}");

        var result = await _parkingLotRepository.GetParkingLotWithCarSpaces(parkingLotId);

        return ServiceResponse<ParkingLotWithCarSpacesResponse>.Ok(result);
    }
    public async Task<ServiceResponse<ParkingLotResponse>> CreateParkingLot(ParkingLotRequest request)
    {
        var existUser = await _userAccountRepository.UserExistById(request.UserId);

        if (!existUser)
            return ServiceResponse<ParkingLotResponse>.Fail("Usuário inválido");

        var newParkingLot = await _parkingLotRepository.CreateNewParkingLot(request);

        return ServiceResponse<ParkingLotResponse>.Ok(new ParkingLotResponse
        {
            Id = newParkingLot.Id,
            Name = newParkingLot.Name,
            Address = newParkingLot.Address,
            FirstHourAmount = newParkingLot.FirstHourAmount,
            ExtraHourAmount = newParkingLot.ExtraHourAmount,
            IsActive = newParkingLot.IsActive,
            UserId = newParkingLot.UserId
        });
    }

    public async Task<ServiceResponse<ParkingLotResponse>> UpdateParkingLot(Guid parkingLotId, ParkingLotRequest request)
    {
        var existUser = await _userAccountRepository.UserExistById(request.UserId);

        if (!existUser)
            return ServiceResponse<ParkingLotResponse>.Fail("Usuário inválido");

        var updateParkingLot = await _parkingLotRepository.UpdateParkingLot(parkingLotId, request);

        if (!updateParkingLot)
            return ServiceResponse<ParkingLotResponse>.Fail("Não foi possível atualizar os dados");

        var updated = await _parkingLotRepository.GetParkingLotById(parkingLotId);

        return ServiceResponse<ParkingLotResponse>.Ok(new ParkingLotResponse
        {
            Id = updated.Id,
            Name = updated.Name,
            Address = updated.Address,
            FirstHourAmount = updated.FirstHourAmount,
            ExtraHourAmount = updated.ExtraHourAmount,
            IsActive = updated.IsActive,
            UserId = updated.UserId
        });
    }
    
    public async Task<ServiceResponse<bool>> DeleteParkingLot(Guid parkingLotId, Guid userId)
    {
        if (parkingLotId == Guid.Empty || userId == Guid.Empty)
            return ServiceResponse<bool>.Fail("Id Inválidos");

        var existParkingLot = await _parkingLotRepository.ExistParkingById(parkingLotId);
        var existUser = await _userAccountRepository.UserExistById(userId);

        if (!existUser || !existParkingLot)
            return ServiceResponse<bool>.Fail("Usuário ou Estacionamento inválido");

        var parkingLotContainsInUser = await _parkingLotRepository.ParkingContainsInUser(parkingLotId, userId);

        if(!parkingLotContainsInUser)
            return ServiceResponse<bool>.Fail("Estacionamento não pertence ao usuário");

        var deleted = await _parkingLotRepository.DeleteParking(parkingLotId);

        if (!deleted)
            return ServiceResponse<bool>.Fail("Não foi possível deletar o estacionamento");

        return ServiceResponse<bool>.Ok(true);
    }
}
