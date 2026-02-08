using DesafioHostway.Interfaces.Repository;
using DesafioHostway.Interfaces.Service;
using DesafioHostway.Models.Request;
using DesafioHostway.Models.Response;
using DesafioHostway.Models.Response.Wrappers;

namespace DesafioHostway.Services;

public class CarSpaceService(ICarSpaceRepository carSpaceRepository, IParkingLotRepository parkingLotRepository) : ICarSpaceService
{

    private readonly ICarSpaceRepository _carSpaceRepository = carSpaceRepository;
    private readonly IParkingLotRepository _parkingLotRepository = parkingLotRepository;

    public async Task<ServiceResponse<IEnumerable<CarSpaceResponse>>> GetAllSpacesByParkingLots(Guid parkingLotId)
    {
        if (parkingLotId == Guid.Empty)
            return ServiceResponse<IEnumerable<CarSpaceResponse>>.Fail("Id Inválido");

        var result = await _carSpaceRepository.GetAllSpacesByParkingLot(parkingLotId);

        if (result == null || !result.Any())
            return ServiceResponse<IEnumerable<CarSpaceResponse>>.Ok("Sem Data");

        return ServiceResponse<IEnumerable<CarSpaceResponse>>.Ok(result);
    }

    public async Task<ServiceResponse<CarSpaceResponse>> GetSpaceById(Guid spaceId)
    {
        if (spaceId == Guid.Empty)
            return ServiceResponse<CarSpaceResponse>.Fail("Id Inválido");
        var existCarSpace = await _carSpaceRepository.GetCarSpaceById(spaceId);

        if (existCarSpace == null)
            return ServiceResponse<CarSpaceResponse>.Fail($"Espaço não encontrado com o Id fornecido: {spaceId}");

        return ServiceResponse<CarSpaceResponse>.Ok(existCarSpace);
    }
    
    public async Task<ServiceResponse<CarSpaceResponse>> CreateNewCarSpace(CarSpaceRequest request)
    {
        var existParkingLot = await _parkingLotRepository.ExistParkingById(request.ParkingLotId);

        if (!existParkingLot)
            return ServiceResponse<CarSpaceResponse>.Fail("Estacionamento inválido");

        var newCarSpace = await _carSpaceRepository.CreateCarSpace(request);

        return ServiceResponse<CarSpaceResponse>.Ok(new CarSpaceResponse
        {
            Id = newCarSpace.Id,
            SpaceNumber = newCarSpace.SpaceNumber,
            ParkingLotId = newCarSpace.ParkingLotId,
            IsAccessibleParkingSpace = newCarSpace.IsAccessibleParkingSpace,
            IsActive = newCarSpace.IsActive
        });
    }
    
    public async Task<ServiceResponse<CarSpaceResponse>> UpdateCarSpace(Guid CarSpaceId, CarSpaceRequest request)
    {
        var existCarSpace = await _carSpaceRepository.ExistSpaceById(CarSpaceId);

        if (!existCarSpace)
            return ServiceResponse<CarSpaceResponse>.Fail("Estacionamento inválido");

        var updateCarSpace = await _carSpaceRepository.UpdateCarSpace(CarSpaceId, request);

        if (!updateCarSpace)
            return ServiceResponse<CarSpaceResponse>.Fail("Não foi possível atualizar os dados");

        var updated = await _carSpaceRepository.GetCarSpaceById(CarSpaceId);

        return ServiceResponse<CarSpaceResponse>.Ok(new CarSpaceResponse
        {
            Id = updated.Id,
            SpaceNumber = updated.SpaceNumber,
            ParkingLotId = updated.ParkingLotId,
            IsAccessibleParkingSpace = updated.IsAccessibleParkingSpace,
            IsActive = updated.IsActive
        });
    }

    public async Task<ServiceResponse<bool>> DeleteSpace(Guid spaceId, Guid parkingLotId)
    {
        if (spaceId == Guid.Empty || parkingLotId == Guid.Empty)
            return ServiceResponse<bool>.Fail("Id Inválidos");

        var existSpace = await _carSpaceRepository.ExistSpaceById(spaceId);
        var existParkingLot = await _parkingLotRepository.ExistParkingById(parkingLotId);

        if (!existSpace || !existParkingLot)
            return ServiceResponse<bool>.Fail("Vaga ou Estacionamento são inválido");

        var spaceContainsInParkingLot = await _carSpaceRepository.CarSpaceContainsInParkingLot(spaceId, parkingLotId);

        if (!spaceContainsInParkingLot)
            return ServiceResponse<bool>.Fail("Espaço não pertence ao estacionamento");

        var deleted = await _carSpaceRepository.DeleteCarSpace(spaceId);

        if (!deleted)
            return ServiceResponse<bool>.Fail("Não foi possível deletar o espaço");

        return ServiceResponse<bool>.Ok(true);
    }
}
