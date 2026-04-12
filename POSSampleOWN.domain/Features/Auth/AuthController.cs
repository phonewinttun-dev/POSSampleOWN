using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using POSSampleOWN.domain.Features.Auth;
using POSSampleOWN.DTOs;
using POSSampleOWN.Responses;
using Microsoft.AspNetCore.Http;

namespace POSSampleOWN.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserRegisterService _registerService;

    public AuthController(IAuthService authService, IUserRegisterService registerService)
    {
        _authService = authService;
        _registerService = registerService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Invalid registration data."));

        var result = await _registerService.RegisterAsync(request);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [AllowAnonymous]
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

        // Store refresh token in HttpOnly cookie
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7) 
        };
        Response.Cookies.Append("refreshToken", result.RefreshToken, cookieOptions);

        // Clear refresh token from response body
        result.RefreshToken = string.Empty;

        return Ok(ApiResponse<TokenResponse>.Success(result, "Login successful."));
    }
}
