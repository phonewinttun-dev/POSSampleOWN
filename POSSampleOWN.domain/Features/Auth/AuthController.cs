using Microsoft.AspNetCore.Mvc;
using POSSampleOWN.domain.Features.Auth;
using POSSampleOWN.shared.DTOs.Auth;
using POSSampleOWN.Responses;

namespace POSSampleOWN.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Invalid login data."));

        var result = await _authService.LoginAsync(request);

        if (result == null)
        {
            return Unauthorized(ApiResponse<object>.Fail("Invalid email or password."));
        }

        return Ok(ApiResponse<TokenResponse>.Success(result, "Login successful."));
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        if (string.IsNullOrEmpty(request.RefreshToken))
            return BadRequest(ApiResponse<object>.Fail("Refresh token is required."));

        var result = await _authService.RefreshTokenAsync(request.RefreshToken);

        if (result == null)
        {
            return Unauthorized(ApiResponse<object>.Fail("Invalid or expired refresh token."));
        }

        return Ok(ApiResponse<TokenResponse>.Success(result, "Token refreshed successfully."));
    }
}
