using DesafioHostway.Models.Request;
using DesafioHostway.Models.Response;

namespace DesafioHostway.Interfaces.Repository;

public interface IUserAccountRepository
{
    Task<bool> UserExistById(Guid userId);
    Task<UserAccountResponse?> GetUserInfo(Guid userId);
    Task<bool> UpdateUserInfo(Guid userId, UserAccountRequest request);
    Task<bool> DeleteUserAccount(Guid userId);
}
