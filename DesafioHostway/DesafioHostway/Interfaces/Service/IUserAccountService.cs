using DesafioHostway.Models.Request;
using DesafioHostway.Models.Response;
using DesafioHostway.Models.Response.Wrappers;

namespace DesafioHostway.Interfaces.Service;

public interface IUserAccountService
{
    Task<ServiceResponse<UserAccountResponse?>> GetUserInfo(Guid userId);
    Task<ServiceResponse<bool>> UpdateUserInfo(Guid Userid, UserAccountRequest request);
    Task<ServiceResponse<bool>> DeleteUserAccount(Guid userId);
}
