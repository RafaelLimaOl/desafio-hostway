using DesafioHostway.Interfaces.Service;
using DesafioHostway.Models.Request;
using DesafioHostway.Models.Response;
using DesafioHostway.Models.Response.Wrappers;
using DesafioHostway.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesafioHostway.Controllers;

[ApiController]
[Route("api/account")]
public class UserAccountController(IUserAccountService userAccountService) : ControllerBase
{

    private readonly IUserAccountService _userAccountService = userAccountService;

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUserAccountInfo()
    {
        var userId = User.GetUserId();
        var result = await _userAccountService.GetUserInfo(userId);
        if (!result.Success)
            return NotFound(new ApiResponse<UserAccountResponse>(false, result.Message!, null));

        return Ok(new ApiResponse<UserAccountResponse>(true, "Informações da conta retornadas com sucesso!", result.Data));
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateUserAccountInfo(UserAccountRequest request)
    {
        var userId = User.GetUserId();
        var result = await _userAccountService.UpdateUserInfo(userId, request);

        if (!result.Success)
        {
            if (result.Message!.Contains("não encontrado"))
                return NotFound(new ApiResponse<bool>(false, result.Message, false));

            return BadRequest(new ApiResponse<bool>(false, result.Message!, false));
        }

        return Ok(new ApiResponse<bool>(true, "Conta atualizado com sucesso!", true));
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteUserAccount()
    {
        var userId = User.GetUserId();

        var result = await _userAccountService.DeleteUserAccount(userId);

        if (!result.Success)
            return BadRequest(new ApiResponse<bool>(false, result.Message!, false));

        return Ok(new ApiResponse<bool>(true, "Conta deletada com sucesso", true));
    }
}
