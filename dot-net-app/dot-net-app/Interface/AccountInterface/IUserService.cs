﻿using dot_net_app.Model.AccountModel;
using dot_net_app.Model.Shared;
using Microsoft.AspNetCore.Mvc;

namespace dot_net_app.Interface.AccountInterface
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int userId);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByEmailAsync(string email);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int userId);

        // OTP Verification
        Task<bool> VerifyOtp(string username, string otp);

        Task ResendOTP(string username, User userData);

        // User Registration
        Task<User> CreateUserAsync(CreateUserRequest createUserRequest);

        // User Login
        Task<User?> GetUserByUsernameAndPasswordAsync(string usernameOrEmail, string password);
    }
}
