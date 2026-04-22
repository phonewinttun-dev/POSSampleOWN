using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POSSampleOWN.domain.DTOs;
using POSSampleOWN.domain.Features.Point;
using POSSampleOWN.Responses;
using POSSampleOWN.shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POSSampleOWN;

[Route("api/points")]
[ApiController]
public class PointController : ControllerBase
{
    private readonly IPointService _service;

    public PointController(IPointService service)
    {
        _service = service;
    }
    [AllowAnonymous]
    [HttpPost("accounts")]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountReqDTO request)
    {
        if (!ModelState.IsValid) return BadRequest(ApiResponse<object>.Fail("Invalid request data."));

        var result = await _service.CreateAccountAsync(request);
        return Ok(result);
    }
    [AllowAnonymous]
    [HttpGet("accounts")]
    public async Task<IActionResult> GetAccounts([FromQuery] AccountListReqDTO request)
    {
        var result = await _service.GetAccountsAsync(request);
        return Ok(result);
    }
    [AllowAnonymous]
    [HttpGet("accounts/lookup/{userId}")]
    public async Task<IActionResult> LookupAccount(string userId)
    {
        var result = await _service.LookupAccountAsync(userId);
        return Ok(result);
    }
    [AllowAnonymous]
    [HttpGet("balance-lookup")]
    public async Task<IActionResult> GetBalance([FromQuery] CheckBalanceReqDTO request)
    {
        var result = await _service.GetUserBalanceAsync(request);
        return Ok(result);
    }
    [AllowAnonymous]
    [HttpPost("earn")]
    public async Task<IActionResult> EarnPoints([FromBody] EarnPointReqDTO request)
    {
        if (!ModelState.IsValid) return BadRequest(ApiResponse<object>.Fail("Invalid request data."));

        var result = await _service.EarnPointsAsync(request);
        return Ok(result);
    }
    [AllowAnonymous]
    [HttpGet("rewards/available")]
    public async Task<IActionResult> GetRewards()
    {
        var result = await _service.GetAvailableRewardsAsync();
        return Ok(result);
    }
    [AllowAnonymous]
    [HttpPost("redemption/claim")]
    public async Task<IActionResult> ClaimReward([FromBody] ClaimRewardReqDTO request)
    {
        if (!ModelState.IsValid) return BadRequest(ApiResponse<object>.Fail("Invalid request data."));

        var result = await _service.ClaimRewardAsync(request);
        return Ok(result);
    }
    [AllowAnonymous]
    [HttpGet("accounts/{accountId}/history")]
    public async Task<IActionResult> GetHistory(string accountId)
    {
        var result = await _service.GetPointHistoryAsync(accountId);
        return Ok(result);
    }
    [AllowAnonymous]
    [HttpGet("admin/redemptions/pending")]
    public async Task<IActionResult> GetPendingRedemptions()
    {
        var result = await _service.GetPendingRedemptionsAsync();
        return Ok(result);
    }
    [AllowAnonymous]
    [HttpPut("admin/redemptions/{id}/status")]
    public async Task<IActionResult> UpdateStatus(string id, [FromBody] RedemptionStatus status)
    {
        var result = await _service.UpdateRedemptionStatusAsync(id, status);
        return Ok(result);
    }
}