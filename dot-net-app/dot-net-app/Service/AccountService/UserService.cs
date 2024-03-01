using dot_net_app.Interface.AccountInterface;
using dot_net_app.Interface.EmailServiceInterface;
using dot_net_app.Model.AccountModel;
using Microsoft.Extensions.Caching.Memory;

namespace dot_net_app.Service.AccountService
{
    public class UserService(IUserRepository userRepository, IMemoryCache memoryCache, IEmailService emailService) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly IEmailService _emailService = emailService;

        private static string GenerateOTP()
        {
            Random random = new();
            int otpNumber = random.Next(100000, 999999);
            return otpNumber.ToString();
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

        public Task<bool> VerifyOtp(string username, string otp)
        {
            if (_memoryCache.TryGetValue<string>(username, out var cachedOTP))
            {
                _memoryCache.Remove(username);
                return Task.FromResult(otp == cachedOTP);
            }
            return Task.FromResult(false);
        }

        // User Registration
        public async Task<User> CreateUserAsync(CreateUserRequest createUserRequest)
        {
            if (!createUserRequest.IsValid())
            {
                throw new ArgumentException("Invalid user creation request");
            }

            string otp = GenerateOTP();

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

            if (user.Username != null)
            {
                _memoryCache.Set(user.Username, otp, TimeSpan.FromMinutes(10));
            }

            string EmailSubject = "OTP Verification";

            string HtmlBody = @"
                        <!DOCTYPE html>
                        <html lang=""en"">
                        <head>
                            <meta charset=""UTF-8"">
                            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                            <title>Registration OTP</title>
                            <style>
                                body, h1, p {
                          margin: 0;
                          padding: 0;
                          font-family: 'Segoe UI', Tahoma, sans-serif; 
                        }

                        .container {
                          max-width: 600px;
                          margin: 20px auto; 
                          padding: 20px;
                          background: linear-gradient(135deg, cyan, #32CD32); 
                          border-radius: 10px;
                          box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
                          color: #ffffff;
                          text-align: center;
                        }

                        .otp {
                          background-color: #ffffff;
                          color: #333333; 
                          padding: 15px 30px;
                          border-radius: 5px;
                          font-size: 32px;
                          margin: 20px 0;
                          box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                          font-family: monospace; 
                        }

                        .social-link {
                          margin-top: 20px;
                          display: block;
                        }

                        .social-link a { 
                          color: #ffeb3b; 
                          text-decoration: none;
                          display: inline-block;
                          transition: all 0.3s ease-in-out;
                        }

                        .social-link img {
                          width: 24px;
                          height: 24px;
                          vertical-align: middle;
                        }

                        .social-link a:hover {
                          opacity: 0.8; 
                        }
                            </style>
                        </head>
                        <body>
                          <div class=""container"">
                            <h1>Welcome to Our Website!</h1>
                            <p>Thank you for registering. Your One-Time Password (OTP) is:</p>
                            <div class=""otp"">" + otp + 
                            @"</div>
                            <p>Please use this OTP to complete your registration.</p>

                            <p class=""social-link"">
                              <a href=""https://www.linkedin.com/in/yuvraj96k/"" target=""_blank"">
                                <img src=""https://upload.wikimedia.org/wikipedia/commons/thumb/c/ca/LinkedIn_logo_initials.png/600px-LinkedIn_logo_initials.png"" alt=""LinkedIn Icon"">
                              </a>
                            </p>
                          </div>
                        </body>
                        </html>";

            if (user.Email != null && user.FullName != null)
            {
                await _emailService.SendEmailAsync(user.FullName, user.Email, EmailSubject, HtmlBody);
            }

            return user;
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
