using POSSampleOWN.Responses;
using POSSampleOWN.DTOs;
using System.Threading.Tasks;

namespace POSSampleOWN.domain.Features.Auth
{
    public interface IUserRegisterService
    {
        Task<ApiResponse<UserRegisterResponse>> RegisterAsync(UserRegisterRequest request);
        bool IsValidEmail(string email);
    }
}
