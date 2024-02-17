using dot_net_app.Model.AccountModel;

namespace dot_net_app.Interface.AccountInterface
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int userId);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByEmailAsync(string email);
        Task<int> CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int userId);
        Task<List<User>> GetAllUsersAsync();
    }
}
