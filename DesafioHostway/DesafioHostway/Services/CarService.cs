using DesafioHostway.Interfaces.Repository;
using DesafioHostway.Interfaces.Service;
using DesafioHostway.Models.Request;
using DesafioHostway.Models.Response;
using DesafioHostway.Models.Response.Wrappers;

namespace DesafioHostway.Services;

public class CarService(ICarRepository carRepository, IUserAccountRepository userAccountRepository) : ICarService
{
    private readonly ICarRepository _carRepository = carRepository;
    private readonly IUserAccountRepository _userAccountRepository = userAccountRepository;

    public async Task<ServiceResponse<IEnumerable<CarResponse>>> GetAllCars(Guid userId)
    {
        if (userId == Guid.Empty)
            return ServiceResponse<IEnumerable<CarResponse>>.Fail("Id Inválido");

        var result = await _carRepository.GetAllCars(userId);

        if (result == null || !result.Any())
            return ServiceResponse<IEnumerable<CarResponse>>.Ok("Sem Data");

        return ServiceResponse<IEnumerable<CarResponse>>.Ok(result);
    } 

    public async Task<ServiceResponse<CarResponse>> GetCarById(Guid carId)
    {
        if (carId == Guid.Empty)
            return ServiceResponse<CarResponse>.Fail("Id Inválido");

        var existCar = await _carRepository.GetCarById(carId);

        if (existCar == null)
            return ServiceResponse<CarResponse>.Fail($"Carro não encontrado com o Id fornecido: {carId}");

        return ServiceResponse<CarResponse>.Ok(existCar);
    }

    public async Task<ServiceResponse<CarResponse>> CreateCar(CarRequest request)
    {
        var existUser = await _userAccountRepository.UserExistById(request.UserId);
        var existCarByLicensePlate = await _carRepository.CarExistsByLicensePlate(null, request.LicensePlate);

        if (!existUser)
            return ServiceResponse<CarResponse>.Fail("Usuário inválido");

        if (existCarByLicensePlate)
            return ServiceResponse<CarResponse>.Fail("Essa placa de carro já foi registrada");

        var newCar = await _carRepository.CreateCar(request);

        return ServiceResponse<CarResponse>.Ok(new CarResponse
        {
            Id = newCar.Id,
            Color = newCar.Color,
            LicensePlate = newCar.LicensePlate,
            Model = newCar.Model,
            IsActive = newCar.IsActive,
            UserId = newCar.UserId
        });
    }

    public async Task<ServiceResponse<CarResponse>> UpdateCar(Guid carId, CarRequest request)
    {
        var existUser = await _userAccountRepository.UserExistById(request.UserId);
        var carContainsInUser = await _carRepository.CarContainsInUser(carId, request.UserId);
        var existCarByLicensePlate = await _carRepository.CarExistsByLicensePlate(carId, request.LicensePlate);

        if (!existUser || !carContainsInUser)
            return ServiceResponse<CarResponse>.Fail("Usuário não encontrado ou Carro não pertence ao usuário!");

        if (existCarByLicensePlate)
            return ServiceResponse<CarResponse>.Fail("Essa placa de carro já foi registrada");

        var updated = await _carRepository.UpdateCar(carId, request);

        if (!updated)
            return ServiceResponse<CarResponse>.Fail("Não foi possível atualizar os dados");

        var updatedCar = await _carRepository.GetCarById(carId);

        return ServiceResponse<CarResponse>.Ok(new CarResponse
        {
            Id = updatedCar!.Id,
            Color = updatedCar.Color,
            LicensePlate = updatedCar.LicensePlate,
            Model = updatedCar.Model,
            IsActive = updatedCar.IsActive,
            UserId = updatedCar.UserId
        });
    }

    public async Task<ServiceResponse<bool>> DeleteCar(Guid UserId, Guid CarId)
    {
        var existUser = await _userAccountRepository.UserExistById(UserId);
        var carContainsInUser = await _carRepository.CarContainsInUser(CarId, UserId);

        if (!existUser || !carContainsInUser)
            return ServiceResponse<bool>.Fail("Usuário não encontrado ou Carro não pertence ao usuário!");

        var result = await _carRepository.DeleteCar(CarId);

        if (!result)
            return ServiceResponse<bool>.Fail("Carro com o Id fornecido não foi deletada");

        return ServiceResponse<bool>.Ok("Carro deletada com sucesso");
    }
}
