using dot_net_app.Model.AccountModel;

namespace dot_net_app.Interface.AccountInterface
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int userId);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByEmailAsync(string email);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int userId);

        // User Registration
        Task<User> CreateUserAsync(CreateUserRequest createUserRequest);
    }
}
