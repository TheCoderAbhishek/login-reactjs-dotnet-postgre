using dot_net_app.Model.AccountModel;

namespace dot_net_app.Interface.AccountInterface
{
    /// <summary>
    /// Interface for user service operations.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Retrieves a user by ID asynchronously.
        /// </summary>
        Task<User> GetUserByIdAsync(int userId);

        /// <summary>
        /// Retrieves a user by username asynchronously.
        /// </summary>
        Task<User> GetUserByUsernameAsync(string username);

        /// <summary>
        /// Retrieves a user by email asynchronously.
        /// </summary>
        Task<User> GetUserByEmailAsync(string email);

        /// <summary>
        /// Updates a user asynchronously.
        /// </summary>
        Task UpdateUserAsync(User user);

        /// <summary>
        /// Deletes a user asynchronously.
        /// </summary>
        Task DeleteUserAsync(int userId);

        /// <summary>
        /// Verifies OTP (One-Time Password) asynchronously.
        /// </summary>
        Task<bool> VerifyOtp(string username, string otp);

        /// <summary>
        /// Resends OTP (One-Time Password) asynchronously.
        /// </summary>
        Task ResendOTP(string username, User userData);

        /// <summary>
        /// Creates a new user asynchronously.
        /// </summary>
        Task<User> CreateUserAsync(CreateUserRequest createUserRequest);

        /// <summary>
        /// Retrieves a user by username/email and password asynchronously.
        /// </summary>
        Task<User?> GetUserByUsernameAndPasswordAsync(string usernameOrEmail, string password);

        /// <summary>
        /// Logs out a user.
        /// </summary>
        /// <param name="username">The username of the user to log out.</param>
        /// <returns>A task representing the asynchronous operation of logging out the user.</returns>
        Task Logout(string username);
    }
}
