using dot_net_app.Model.AccountModel;

namespace dot_net_app.Interface.AccountInterface
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int userId);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByEmailAsync(string email);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int userId);
        Task<List<User>> GetAllUsersAsync();

        // User Registration
        Task<User> CreateUserAsync(User user);

        // User Login
        Task<User?> GetUserByUsernameOrEmailAsync(string usernameOrEmail);
    }
}
