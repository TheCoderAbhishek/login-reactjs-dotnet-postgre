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
        public async Task<User?> CreateUserAsync(User user)
        {
            if (user.DateOfBirth.HasValue)
            {
                user.DateOfBirth = user.DateOfBirth.Value.ToUniversalTime();
                user.IsVerified = true;
                user.IsActive = true;
                user.IsAdmin = true;
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            user.PasswordHash = hashedPassword;

            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            user.LastLoginAt = DateTime.UtcNow;

            _accountDbContext.Users.Add(user);
            await _accountDbContext.SaveChangesAsync();
            return user;
        }

        // User Login
        public async Task<User?> GetUserByUsernameOrEmailAsync(string usernameOrEmail)
        {
            return await _accountDbContext.Users.FirstOrDefaultAsync(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail);
        }
    }
}
