using DesafioHostway.Interfaces.Repository;
using DesafioHostway.Interfaces.Service;
using DesafioHostway.Models.Request;
using DesafioHostway.Models.Response;
using DesafioHostway.Models.Response.Wrappers;

namespace DesafioHostway.Services;

public class UserAccountService(IUserAccountRepository userAccountRepository) : IUserAccountService
{

    private readonly IUserAccountRepository _userAccountRepository = userAccountRepository;

    public async Task<ServiceResponse<UserAccountResponse?>> GetUserInfo(Guid userId)
    {
        if (userId == Guid.Empty)
            return ServiceResponse<UserAccountResponse?>.Fail("Id Inválido");

        var result = await _userAccountRepository.GetUserInfo(userId);

        if (result == null)
            return ServiceResponse<UserAccountResponse?>.Ok("Sem Data");

        return ServiceResponse<UserAccountResponse?>.Ok(result);
    }

    public async Task<ServiceResponse<bool>> UpdateUserInfo(Guid userId, UserAccountRequest request)
    {
        if (userId == Guid.Empty)
            return ServiceResponse<bool>.Fail("Id Inválido");

        var existUser = await _userAccountRepository.UserExistById(userId);

        if (!existUser)
            return ServiceResponse<bool>.Fail("Usuário não encontrado!");

        var updated = await _userAccountRepository.UpdateUserInfo(userId, request);

        if (!updated)
            return ServiceResponse<bool>.Fail("Não foi possível atualizar os dados");

        return ServiceResponse<bool>.Ok(true);
    }

    public async Task<ServiceResponse<bool>> DeleteUserAccount(Guid userId)
    {
        var existUser = await _userAccountRepository.UserExistById(userId);

        if (!existUser)
            return ServiceResponse<bool>.Fail("Usuário não encontrado!");

        var result = await _userAccountRepository.DeleteUserAccount(userId);

        if (!result)
            return ServiceResponse<bool>.Fail("Usuário com o Id fornecido não foi deletado");

        return ServiceResponse<bool>.Ok(true);
    }

}
