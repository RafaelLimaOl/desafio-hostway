using DesafioHostway.Interfaces.Service;
using DesafioHostway.Models.Request;
using DesafioHostway.Models.Response;
using DesafioHostway.Models.Response.Wrappers;
using DesafioHostway.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesafioHostway.Controllers;

[ApiController]
[Route("api/session")]
public class SessionController(IParkingSessionService parkingSessionService) : ControllerBase
{
    private readonly IParkingSessionService _parkingSessionService = parkingSessionService;

    [HttpGet("car/{carId}")]
    public async Task<IActionResult> GetSessionByCar(Guid carId)
    {
        var result = await _parkingSessionService.GetSessionByCar(carId);

        if (!result.Success)
            return NotFound(new ApiResponse<ParkingSessionResponse>(false, result.Message!, null));

        return Ok(new ApiResponse<ParkingSessionResponse>(true, "Sessões retornadas com sucesso!", result.Data));
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetSessionByUser()
    {
        var userId = User.GetUserId();
        var result = await _parkingSessionService.GetSessionByUserId(userId);

        if (!result.Success)
            return NotFound(new ApiResponse<IEnumerable<ParkingSessionResponse>>(false, result.Message!, null));

        return Ok(new ApiResponse<IEnumerable<ParkingSessionResponse>>(true, "Sessões retornadas com sucesso!", result.Data));
    }

    [HttpGet("{sessionId}")]
    public async Task<IActionResult> GetById(Guid sessionId)
    {
        var result = await _parkingSessionService.GetSessionById(sessionId);

        if (!result.Success)
            return NotFound(new ApiResponse<ParkingSessionResponse>(false, result.Message!, null));

        return Ok(new ApiResponse<ParkingSessionResponse>(true, "Sessão retornada com sucesso!", result.Data));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> OpenSession( ParkinSessionCreateRequest request)
    {
        var userId = User.GetUserId();

        var result = await _parkingSessionService.OpenNewSession(userId, request);

        if (!result.Success)
            return BadRequest(new ApiResponse<ParkingSessionResponse>(false, result.Message!, null));

        return Ok(new ApiResponse<ParkingSessionResponse>(true, "Sessão iniciada com sucesso!", result.Data));
    }

    [HttpPut("close/{sessionId}")]
    public async Task<IActionResult> CloseSession(Guid sessionId)
    {
        var result = await _parkingSessionService.CloseCurrentSession(sessionId);

        if (!result.Success)
            return BadRequest(new ApiResponse<ParkingSessionResponse>(false, result.Message!, null));

        return Ok(new ApiResponse<ParkingSessionResponse>(true, "Sessão encerrada com sucesso!", result.Data));
    }

    [HttpDelete("{sessionId}")]
    public async Task<IActionResult> DeleteSession(Guid sessionId)
    {
        var result = await _parkingSessionService.DeleteSession(sessionId);

        if (!result.Success)
            return BadRequest(new ApiResponse<bool>(false, result.Message!, false));

        return Ok(new ApiResponse<bool>(true, "Sessão deletada com sucesso", true));
    }


}
