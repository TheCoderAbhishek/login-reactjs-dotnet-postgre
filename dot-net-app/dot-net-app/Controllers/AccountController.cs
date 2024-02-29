using dot_net_app.Interface.AccountInterface;
using dot_net_app.Model.AccountModel;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace dot_net_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;

        public AccountController(IUserRepository userRepository, IUserService userService)
        {
            _userRepository = userRepository;
            _userService = userService;
        }

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

        // POST: api/account/register
        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest createUserRequest)
        {
            try
            {
                var userData = await _userService.CreateUserAsync(createUserRequest);
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

        // api/account/login
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
