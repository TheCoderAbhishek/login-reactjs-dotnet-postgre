﻿using dot_net_app.Interface.AccountInterface;
using dot_net_app.Model.AccountModel;

namespace dot_net_app.Service.AccountService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetUserByUsernameAsync(username);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task DeleteUserAsync(int userId)
        {
            await _userRepository.DeleteUserAsync(userId);
        }

        // User Registration
        public async Task<User?> CreateUserAsync(CreateUserRequest createUserRequest)
        {
            if (!createUserRequest.IsValid())
            {
                throw new ArgumentException("Invalid user creation request");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(createUserRequest.PasswordHash);

            var user = new User
            {
                Username = createUserRequest.Username,
                Email = createUserRequest.Email,
                PasswordHash = hashedPassword,
                FullName = createUserRequest.FullName,
                MobileNumber = createUserRequest.MobileNumber,
                Gender = createUserRequest.Gender,
                DateOfBirth = createUserRequest.DateOfBirth?.ToUniversalTime(),
                IsAdmin = true,
                IsActive = true,
                IsVerified = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                LastLoginAt = DateTime.UtcNow
            };

            return await _userRepository.CreateUserAsync(user);
        }

        // User Login
        public async Task<User?> GetUserByUsernameAndPasswordAsync(string usernameOrEmail, string password)
        {
            var user = await _userRepository.GetUserByUsernameOrEmailAsync(usernameOrEmail);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return user;
            }

            return null;
        }
    }
}
