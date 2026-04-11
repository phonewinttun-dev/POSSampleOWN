using POSSampleOWN.shared.DTOs.Auth;

namespace POSSampleOWN.domain.Features.Auth;

public interface IAuthService
{
    Task<TokenResponse?> LoginAsync(LoginRequest request);
    Task<TokenResponse?> RefreshTokenAsync(string refreshToken);
}
