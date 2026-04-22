using POSSampleOWN.domain.DTOs;
using POSSampleOWN.Responses;
using POSSampleOWN.shared;

namespace POSSampleOWN.domain.Features.Point
{
    public interface IPointService
    {
        Task<ApiResponse<ClaimRewardResDTO>> ClaimRewardAsync(ClaimRewardReqDTO request);
        Task<ApiResponse<CreateAccountResDTO>> CreateAccountAsync(CreateAccountReqDTO request);
        Task<ApiResponse<AccountLookupResponse>> LookupAccountAsync(string userId);
        Task<ApiResponse<EarnPointResDTO>> EarnPointsAsync(EarnPointReqDTO request);
        Task<ApiResponse<AccountListResponseWrapper>> GetAccountsAsync(AccountListReqDTO request);
        Task<ApiResponse<List<AvailableRewardResDTO>>> GetAvailableRewardsAsync();
        Task<ApiResponse<List<PendingRedemptionResDTO>>> GetPendingRedemptionsAsync();
        Task<ApiResponse<List<PointHistoryResDTO>>> GetPointHistoryAsync(string accountId);
        Task<ApiResponse<CheckBalanceResDTO>> GetUserBalanceAsync(CheckBalanceReqDTO request);
        Task<ApiResponse<bool>> UpdateRedemptionStatusAsync(string redemptionId, RedemptionStatus status);
    }
}