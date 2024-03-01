using dot_net_app.Interface.AccountInterface;
using dot_net_app.Model.AccountModel;
using dot_net_app.Model.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace dot_net_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IUserRepository userRepository, IUserService userService) : Controller
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUserService _userService = userService;

        // GET: api/account/users
        [HttpGet("users")]
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

                var userDataJson = TempData["UserDataJson"] as string;

                if (userDataJson == null)
                {
                    return BadRequest("User data not found in TempData.");
                }

                var user = JsonConvert.DeserializeObject<User>(userDataJson);

                if (user == null)
                {
                    return BadRequest("User data could not be deserialized.");
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

        // POST: api/account/register
        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest createUserRequest)
        {
            try
            {
                var userData = await _userService.CreateUserAsync(createUserRequest);

                var userDataJson = JsonConvert.SerializeObject(userData);

                TempData["UserDataJson"] = userDataJson;

                return Ok(userData);
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
                    return BadRequest("Username/email and password are required.");
                }

                var user = await _userService.GetUserByUsernameAndPasswordAsync(userLoginRequest.UsernameOrEmail, userLoginRequest.PasswordHash);
                if (user == null)
                {
                    return Unauthorized("Invalid username/email or password.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
