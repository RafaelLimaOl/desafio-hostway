using DesafioHostway.Interfaces.Service;
using DesafioHostway.Models.Request;
using DesafioHostway.Models.Response;
using DesafioHostway.Models.Response.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace DesafioHostway.Controllers;

[ApiController]
[Route("api/space")]
public class CarSpaceController(ICarSpaceService carSpaceService) : ControllerBase
{

    private readonly ICarSpaceService _carSpaceService = carSpaceService;

    [HttpGet("space/{parkingLotId}")]
    public async Task<IActionResult> GetAllSpaceByParkingLot(Guid parkingLotId)
    {
        var result = await _carSpaceService.GetAllSpacesByParkingLots(parkingLotId);
        if (!result.Success)
            return NotFound(new ApiResponse<IEnumerable<CarSpaceResponse>>(false, result.Message!, null));

        return Ok(new ApiResponse<IEnumerable<CarSpaceResponse>>(true, "Lista de vagas retornada com sucesso!", result.Data));
    }

    [HttpGet("{spaceId}")]
    public async Task<IActionResult> GetSpaceById(Guid spaceId)
    {
        var result = await _carSpaceService.GetSpaceById(spaceId);
        if (!result.Success)
            return NotFound(new ApiResponse<CarSpaceResponse>(false, result.Message!, null));

        return Ok(new ApiResponse<CarSpaceResponse>(true, "Vaga retornada com sucesso!", result.Data));
    }

    [HttpPost]
    public async Task<IActionResult> CreateNewSpace(CarSpaceRequest request)
    {
        var result = await _carSpaceService.CreateNewCarSpace(request);

        if (!result.Success)
            return BadRequest(new ApiResponse<CarSpaceResponse>(false, result.Message!, null));

        return Ok(new ApiResponse<CarSpaceResponse>(true, "Vaga criada com sucesso!", result.Data));
    }

    [HttpPut("{carSpaceId}")]
    public async Task<IActionResult> UpdateSpace(Guid carSpaceId, CarSpaceRequest request)
    {
        var result = await _carSpaceService.UpdateCarSpace(carSpaceId, request);

        if (!result.Success)
        {
            if (result.Message!.Contains("não encontrado"))
                return NotFound(new ApiResponse<CarSpaceRequest>(false, result.Message, null));

            return BadRequest(new ApiResponse<CarSpaceRequest>(false, result.Message!, null));
        }

        return Ok(new ApiResponse<CarSpaceResponse>(true, "Vaga atualizada com sucesso!", result.Data));
    }

    [HttpDelete("{spaceCarId}/{parkingLotId}")]
    public async Task<IActionResult> DeleteSpace(Guid spaceCarId, Guid parkingLotId)
    {
        var result = await _carSpaceService.DeleteSpace(spaceCarId, parkingLotId);

        if (!result.Success)
            return BadRequest(new ApiResponse<bool>(false, result.Message!, false));

        return Ok(new ApiResponse<bool>(true, "Vaga deletada com sucesso", true));
    }

}
