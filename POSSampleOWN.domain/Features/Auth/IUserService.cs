using POSSampleOWN.Responses;
using POSSampleOWN.DTOs;
using System.Threading.Tasks;

namespace POSSampleOWN.domain.Features.Auth
{
    public interface IUserService
    {
        Task<ApiResponse<UserResponse>> RegisterAsync(UserRegisterRequest request);
        Task<ApiResponse<UserResponse>> UpdateAsync(int id, UserUpdateRequest request, int currentUserId);
        Task<ApiResponse<UserResponse>> DeleteAsync(int id);
        Task<ApiResponse<UserResponse>> ChangePasswordAsync(int id, ChangePasswordRequest request, int currentUserId);
        bool IsValidEmail(string email);
    }
}
