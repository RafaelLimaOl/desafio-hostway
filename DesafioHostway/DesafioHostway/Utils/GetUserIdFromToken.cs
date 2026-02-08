using System.Security.Claims;

namespace DesafioHostway.Utils;

public static class GetUserIdFromToken
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var claim = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(claim, out var userId))
            throw new UnauthorizedAccessException("UserId inválido");

        return userId;
    }
}
