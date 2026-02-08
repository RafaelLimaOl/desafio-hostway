using DesafioHostway.Models.Entities;
using DesafioHostway.Models.Request;
using DesafioHostway.Models.Response;

namespace DesafioHostway.Interfaces.Service;

public interface IAuthService
{
    Task<User?> RegisterAsync(RegisterUserRequest request);
    Task<TokenResponse?> LoginAsync(LoginUserRequest request);
    Task<TokenResponse?> RefreshTokenAsync(RefreshTokenRequest request);
}
