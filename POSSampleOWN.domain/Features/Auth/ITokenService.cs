using POSSampleOWN.database.Models;
using System.Security.Claims;

namespace POSSampleOWN.domain.Features.Auth;

public interface ITokenService
{
    string GenerateAccessToken(Tbl_User user);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
