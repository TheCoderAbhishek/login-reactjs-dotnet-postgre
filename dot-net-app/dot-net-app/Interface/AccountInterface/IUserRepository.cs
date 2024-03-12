using dot_net_app.Model.AccountModel;

namespace dot_net_app.Interface.AccountInterface
{
    /// <summary>
    /// Interface for user repository operations.
    /// </summary>
    public interface IUserRepository
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
        /// Retrieves all users asynchronously.
        /// </summary>
        Task<List<User>> GetAllUsersAsync();

        /// <summary>
        /// Creates a new user asynchronously.
        /// </summary>
        Task<User?> CreateUserAsync(User user);

        /// <summary>
        /// Retrieves a user by username/email asynchronously.
        /// </summary>
        Task<User?> GetUserByUsernameOrEmailAsync(string usernameOrEmail);
    }
}
