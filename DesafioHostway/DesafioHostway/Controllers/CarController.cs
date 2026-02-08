using DesafioHostway.Interfaces.Service;
using DesafioHostway.Models.Request;
using DesafioHostway.Models.Response;
using DesafioHostway.Models.Response.Wrappers;
using DesafioHostway.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesafioHostway.Controllers;

[ApiController]
[Route("api/car")]
public class CarController(ICarService carService) : ControllerBase
{
    private readonly ICarService _carService = carService;

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = User.GetUserId();
        var result = await _carService.GetAllCars(userId);
        if (!result.Success)
            return NotFound(new ApiResponse<IEnumerable<CarResponse>>(false, result.Message!, null));

        return Ok(new ApiResponse<IEnumerable<CarResponse>>(true, "Lista de carros retornada com sucesso!", result.Data));
    }

    [HttpGet("{carId}")]
    public async Task<IActionResult> GetById(Guid carId) 
    {
        var result = await _carService.GetCarById(carId);
        return Ok(new ApiResponse<CarResponse>(true, "Carro retornado com sucesso!", result.Data));
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(Guid userId) 
    {
        var result = await _carService.GetAllCars(userId);

        if (!result.Success)
            return NotFound(new ApiResponse<IEnumerable<CarResponse>>(false, result.Message!, null));

        return Ok(new ApiResponse<IEnumerable<CarResponse>>(true, "Carros retornado com sucesso!", result.Data));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateNewCar(CarRequest request)
    {
        var userId = User.GetUserId();
        request.UserId = userId;

        var result = await _carService.CreateCar(request);

        if (!result.Success)
            return BadRequest(new ApiResponse<CarResponse>(false, result.Message!, null));

        return Ok(new ApiResponse<CarResponse>(true, "Carro criado com sucesso!", result.Data));
    }

    [Authorize]
    [HttpPut("{carId}")]
    public async Task<IActionResult> UpdateCar(Guid carId, CarRequest request)
    {
        var userId = User.GetUserId();
        request.UserId = userId;

        var result = await _carService.UpdateCar(carId, request);

        if (!result.Success)
        {
            if (result.Message!.Contains("não encontrado"))
                return NotFound(new ApiResponse<CarResponse>(false, result.Message, null));

            return BadRequest(new ApiResponse<CarResponse>(false, result.Message!, null));
        }

        return Ok(new ApiResponse<CarResponse>(true, "Carro atualizado com sucesso!", result.Data));
    }

    [Authorize]
    [HttpDelete("{carId}")]
    public async Task<IActionResult> DeleteCar(Guid carId)
    {
        var userId = User.GetUserId();

        var result = await _carService.DeleteCar(userId, carId);

        if (!result.Success)
            return BadRequest(new ApiResponse<bool>(false, result.Message!, false));

        return Ok(new ApiResponse<bool>(true, "Carro deletado com sucesso", true));
    }
}
