using Microsoft.EntityFrameworkCore;
using POSSampleOWN.database.Data;
using POSSampleOWN.database.Models;
using POSSampleOWN.Responses;
using POSSampleOWN.DTOs;
using System.Text.RegularExpressions;

namespace POSSampleOWN.domain.Features.Auth
{
    public class UserService : IUserService
    {
        private readonly POSDbContext _context;

        public UserService(POSDbContext context)
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
        public async Task<ApiResponse<UserResponse>> RegisterAsync(UserRegisterRequest request)
        {
            // email validation check
            if (!IsValidEmail(request.Email)) return ApiResponse<UserResponse>.Fail("Invalid email format.");

            var existingUser = await _context.Users
                .AnyAsync(u => u.Email == request.Email && !u.DeleteFlag);

            if (existingUser)
            {
                return ApiResponse<UserResponse>.Fail("User with this email already exists.");
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new Tbl_User
            {
                Name = request.Name.Trim(),
                Email = request.Email.Trim(),
                Password = hashedPassword,
                Role = request.Role,
                CreatedAt = DateTime.UtcNow,
                DeleteFlag = false
            };

            try
            {
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                var responseJson = new UserResponse
                {
                    IsSuccess = true,
                    Message = "User registered successfully.",
                    UserId = newUser.Id
                };

                return ApiResponse<UserResponse>.Success(responseJson, responseJson.Message);
            }
            catch (Exception ex)
            {
                return ApiResponse<UserResponse>.Fail($"An error occurred during registration: {ex.Message}");
            }
        }
        #endregion

        #region edit user profile
        public async Task<ApiResponse<UserResponse>> UpdateAsync(int id, UserUpdateRequest request)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && !u.DeleteFlag);

                if (user == null)
                    return ApiResponse<UserResponse>.Fail("User not found.");

                if (!string.IsNullOrWhiteSpace(request.Name))
                    user.Name = request.Name.Trim();

                if (!string.IsNullOrWhiteSpace(request.Email))
                {
                    if (!IsValidEmail(request.Email))
                        return ApiResponse<UserResponse>.Fail("Invalid email format.");

                    var emailExists = await _context.Users
                        .AnyAsync(u => u.Email == request.Email && u.Id != id && !u.DeleteFlag);

                    if (emailExists)
                        return ApiResponse<UserResponse>.Fail("Email already in use by another user.");

                    user.Email = request.Email.Trim();
                }

                //if (request.Role.HasValue)
                //    user.Role = request.Role.Value;

                //if (!string.IsNullOrWhiteSpace(request.Password))
                //{
                //    user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
                //}

                user.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var response = new UserResponse
                {
                    IsSuccess = true,
                    Message = "User updated successfully.",
                    UserId = user.Id
                };

                return ApiResponse<UserResponse>.Success(response, response.Message);
            }
            catch (Exception ex)
            {
                return ApiResponse<UserResponse>.Fail($"An error occurred during update: {ex.Message}");
            }
        }
        #endregion

        #region change password
        public async Task<ApiResponse<UserResponse>> ChangePasswordAsync(int id, ChangePasswordRequest request)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && !u.DeleteFlag);

                if (user == null)
                    return ApiResponse<UserResponse>.Fail("User not found.");

                if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, user.Password))
                    return ApiResponse<UserResponse>.Fail("Invalid old password.");
             
                user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

                user.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var response = new UserResponse
                {
                    IsSuccess = true,
                    Message = "Password changed successfully.",
                    UserId = user.Id
                };

                return ApiResponse<UserResponse>.Success(response, response.Message);
            }
            catch (Exception ex)
            {
                return ApiResponse<UserResponse>.Fail($"An error occurred while changing password: {ex.Message}");
            }
        }
        #endregion

        #region delete user
        public async Task<ApiResponse<UserResponse>> DeleteAsync(int id)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

                if (user == null || user.DeleteFlag)
                    return ApiResponse<UserResponse>.Fail("User not found or already deleted.");

                user.DeleteFlag = true;
                user.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var response = new UserResponse
                {
                    IsSuccess = true,
                    Message = "User deleted successfully.",
                    UserId = user.Id
                };

                return ApiResponse<UserResponse>.Success(response, response.Message);
            }
            catch (Exception ex)
            {
                return ApiResponse<UserResponse>.Fail($"An error occurred during deletion: {ex.Message}");
            }
        }
        #endregion

    }
}
