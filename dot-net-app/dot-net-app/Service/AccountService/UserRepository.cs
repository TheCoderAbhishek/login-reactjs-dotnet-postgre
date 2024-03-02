using dot_net_app.Data;
using dot_net_app.Interface.AccountInterface;
using dot_net_app.Model.AccountModel;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Konscious.Security.Cryptography;

namespace dot_net_app.Service.AccountService
{
    public class UserRepository : IUserRepository
    {
        private readonly AccountDbContext _accountDbContext;
        private readonly IConfiguration _configuration;

        public UserRepository(AccountDbContext accountDbContext, IConfiguration configuration)
        {
            _accountDbContext = accountDbContext;
            _configuration = configuration;
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

            string? saltBase64 = _configuration["Argon2Settings:Salt"];

            if (saltBase64 == null)
            {
                throw new InvalidOperationException("Salt not found in configuration.");
            }

            byte[] salt = Convert.FromBase64String(saltBase64);

            if (user.Username != null)
            {
                using (var argon2 = new Argon2id(Encoding.UTF8.GetBytes(user.Username)))
                {
                    argon2.Salt = salt;
                    argon2.DegreeOfParallelism = 4;
                    argon2.MemorySize = 4096;
                    argon2.Iterations = 3;
                    user.PasswordHash = Convert.ToBase64String(argon2.GetBytes(32));
                }
            }

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
