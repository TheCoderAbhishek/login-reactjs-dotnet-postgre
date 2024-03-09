using dot_net_app.Interface.AccountInterface;
using dot_net_app.Model.AccountModel;
using dot_net_app.Model.Shared;
using Microsoft.AspNetCore.Mvc;

namespace dot_net_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IUserRepository userRepository, IUserService userService, ILogger<AccountController> logger) : ControllerBase
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUserService _userService = userService;
        private readonly ILogger<AccountController> _logger = logger;

        // GET: api/account/users
        [HttpGet("getAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userRepository.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/account/verify-otp
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOTP([FromBody] VerifyOtpRequest verifyOtpRequest)
        {
            try
            {
                if (verifyOtpRequest == null || string.IsNullOrWhiteSpace(verifyOtpRequest.Username) || string.IsNullOrWhiteSpace(verifyOtpRequest.Otp))
                {
                    return BadRequest("Username and OTP are required.");
                }

                var user = verifyOtpRequest.User;

                if (user == null)
                {
                    return BadRequest("User data not found in TempData.");
                }

                bool isOTPVerified = await _userService.VerifyOtp(verifyOtpRequest.Username, verifyOtpRequest.Otp);

                if (isOTPVerified)
                {
                    await _userRepository.CreateUserAsync(user);
                    return Ok(new { message = "OTP verification successful" });
                }
                else
                {
                    return BadRequest(new { message = "Invalid OTP" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred while verifying OTP: {ex}" });
            }
        }

        // POST: api/account/resend-otp
        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOTP([FromBody] ResendOtpRequest resendOtpRequest)
        {
            try
            {
                if (resendOtpRequest == null || string.IsNullOrWhiteSpace(resendOtpRequest.Username))
                {
                    return BadRequest("Username is required.");
                }

                var userData = resendOtpRequest.User;

                if (userData is not null)
                {
                    await _userService.ResendOTP(resendOtpRequest.Username, userData);
                }

                return Ok(new { message = "New OTP sent successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while resending OTP: {ex.Message}");
            }
        }

        // POST: api/account/register
        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest createUserRequest)
        {
            try
            {
                var userData = await _userService.CreateUserAsync(createUserRequest);
                return Ok(new { message = "User created successfully", user = userData });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/account/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest userLoginRequest)
        {
            try
            {
                if (userLoginRequest == null || string.IsNullOrWhiteSpace(userLoginRequest.UsernameOrEmail) || string.IsNullOrWhiteSpace(userLoginRequest.PasswordHash))
                {
                    _logger.LogError("Username/email and password are empty or null.");
                    return BadRequest("Username/email and password are required.");
                }

                var user = await _userService.GetUserByUsernameAndPasswordAsync(userLoginRequest.UsernameOrEmail, userLoginRequest.PasswordHash);
                if (user == null)
                {
                    _logger.LogError("Invalid login attempt: Username/email and password are empty or null.");
                    return Unauthorized("Invalid username/email or password.");
                }
                _logger.LogInformation("User successfully logged into application with valid credentials.");

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
