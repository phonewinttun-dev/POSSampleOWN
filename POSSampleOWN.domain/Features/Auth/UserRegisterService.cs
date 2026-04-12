using Microsoft.EntityFrameworkCore;
using POSSampleOWN.database.Data;
using POSSampleOWN.database.Models;
using POSSampleOWN.Responses;
using POSSampleOWN.DTOs;
using System.Text.RegularExpressions;

namespace POSSampleOWN.domain.Features.Auth
{
    public class UserRegisterService : IUserRegisterService
    {
        private readonly POSDbContext _context;

        public UserRegisterService(POSDbContext context)
        {
            _context = context;
        }

        #region email validation
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase,
                    TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        #endregion

        #region user registration
        public async Task<ApiResponse<UserRegisterResponse>> RegisterAsync(UserRegisterRequest request)
        {
            // email validation check
            if (!IsValidEmail(request.Email)) return ApiResponse<UserRegisterResponse>.Fail("Invalid email format.");

            var existingUser = await _context.Users
                .AnyAsync(u => u.Email == request.Email && !u.DeleteFlag);

            if (existingUser)
            {
                return ApiResponse<UserRegisterResponse>.Fail("User with this email already exists.");
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new Tbl_User
            {
                Name = request.Name,
                Email = request.Email,
                Password = hashedPassword,
                Role = request.Role,
                CreatedAt = DateTime.UtcNow,
                DeleteFlag = false
            };

            try
            {
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                var responseJson = new UserRegisterResponse
                {
                    IsSuccess = true,
                    Message = "User registered successfully.",
                    UserId = newUser.Id
                };

                return ApiResponse<UserRegisterResponse>.Success(responseJson, responseJson.Message);
            }
            catch (Exception ex)
            {
                return ApiResponse<UserRegisterResponse>.Fail($"An error occurred during registration: {ex.Message}");
            }
        }
        #endregion
    }
}
