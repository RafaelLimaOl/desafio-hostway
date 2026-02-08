using DesafioHostway.Interfaces.Service;
using DesafioHostway.Models.Request;
using DesafioHostway.Models.Response;
using DesafioHostway.Models.Response.Wrappers;
using DesafioHostway.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesafioHostway.Controllers;

[ApiController]
[Route("api/parking-lot")]
public class ParkingLotsController(IParkingLotService parkingLotService) : ControllerBase
{
    private readonly IParkingLotService _parkingLotService = parkingLotService;

    [Authorize("admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllParkingLot()
    {
        var userId = User.GetUserId();
        var result = await _parkingLotService.GetAllParkingLots(userId);
        if (!result.Success)
            return NotFound(new ApiResponse<IEnumerable<ParkingLotResponse>>(false, result.Message!, null));

        return Ok(new ApiResponse<IEnumerable<ParkingLotResponse>>(true, "Lista de estacionamento retornada com sucesso!", result.Data));
    }

    [HttpGet("session")]
    public async Task<IActionResult> GetAllAvaliableParkingLot()
    {
        var result = await _parkingLotService.GetAllAvaliableParkingLots();
        if (!result.Success)
            return NotFound(new ApiResponse<IEnumerable<ParkingLotResponse>>(false, result.Message!, null));

        return Ok(new ApiResponse<IEnumerable<ParkingLotResponse>>(true, "Lista de estacionamento retornada com sucesso!", result.Data));
    }

    [HttpGet("{parkingLotId}")]
    public async Task<IActionResult> GetParkingLotById(Guid parkingLotId)
    {
        var result = await _parkingLotService.GetParkingLotById(parkingLotId);
        if (!result.Success)
            return NotFound(new ApiResponse<ParkingLotResponse>(false, result.Message!, null));

        return Ok(new ApiResponse<ParkingLotResponse>(true, "Estacionamento retornado com sucesso!", result.Data));
    }

    [HttpGet("with-spaces/{parkingLotId}")]
    public async Task<IActionResult> GetParkingLotWithCarSpaces(Guid parkingLotId)
    {
        var result = await _parkingLotService.GetParkingLotAndCarSpaces(parkingLotId);
        if (!result.Success)
            return NotFound(new ApiResponse<ParkingLotWithCarSpacesResponse>(false, result.Message!, null));

        return Ok(new ApiResponse<ParkingLotWithCarSpacesResponse>(true, "Estacionameto e vagas retornado com sucesso!", result.Data));
    }

    [Authorize("admin")]
    [HttpPost]
    public async Task<IActionResult> CreateNewParkingLot(ParkingLotRequest request)
    {
        var userId = User.GetUserId();
        request.UserId = userId;

        var result = await _parkingLotService.CreateParkingLot(request);

        if (!result.Success)
            return BadRequest(new ApiResponse<ParkingLotResponse>(false, result.Message!, null));

        return Ok(new ApiResponse<ParkingLotResponse>(true, "Estacionamento criado com sucesso!", result.Data));
    }

    [Authorize("admin")]
    [HttpPut("{parkingLotId}")]
    public async Task<IActionResult> UpdateSpace(Guid parkingLotId, ParkingLotRequest request)
    {
        var userId = User.GetUserId();
        request.UserId = userId;
        var result = await _parkingLotService.UpdateParkingLot(parkingLotId, request);

        if (!result.Success)
        {
            if (result.Message!.Contains("não encontrado"))
                return NotFound(new ApiResponse<ParkingLotResponse>(false, result.Message, null));

            return BadRequest(new ApiResponse<ParkingLotResponse>(false, result.Message!, null));
        }

        return Ok(new ApiResponse<ParkingLotResponse>(true, "Estacionamento atualizado com sucesso!", result.Data));
    }

    [Authorize("admin")]
    [HttpDelete("{parkingLotId}")]
    public async Task<IActionResult> DeleteSpace(Guid parkingLotId)
    {
        var userId = User.GetUserId();

        var result = await _parkingLotService.DeleteParkingLot(parkingLotId, userId);

        if (!result.Success)
            return BadRequest(new ApiResponse<bool>(false, result.Message!, false));

        return Ok(new ApiResponse<bool>(true, "Estacionamento deletado com sucesso", true));
    }

}
