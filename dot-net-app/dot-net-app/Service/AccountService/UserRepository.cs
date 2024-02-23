using dot_net_app.Data;
using dot_net_app.Interface.AccountInterface;
using dot_net_app.Model.AccountModel;
using Microsoft.EntityFrameworkCore;

namespace dot_net_app.Service.AccountService
{
    public class UserRepository : IUserRepository
    {
        private readonly AccountDbContext _accountDbContext;

        public UserRepository(AccountDbContext accountDbContext)
        {
            _accountDbContext = accountDbContext;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            var user = await _accountDbContext.Users.FindAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException($"User with ID {userId} not found.");
            }
            return user;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var user = await _accountDbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                throw new InvalidOperationException($"User with username {username} not found.");
            }
            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _accountDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                throw new InvalidOperationException($"User with email {email} not found.");
            }
            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            _accountDbContext.Users.Update(user);
            await _accountDbContext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _accountDbContext.Users.FindAsync(userId);
            if (user != null)
            {
                _accountDbContext.Users.Remove(user);
                await _accountDbContext.SaveChangesAsync();
            }
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _accountDbContext.Users.ToListAsync();
        }

        // User Registration
        public async Task<User> CreateUserAsync(User user)
        {
            _accountDbContext.Users.Add(user);
            await _accountDbContext.SaveChangesAsync();
            return user;
        }
    }
}
