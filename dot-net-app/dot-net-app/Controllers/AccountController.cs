using dot_net_app.Interface.AccountInterface;
using dot_net_app.Model.AccountModel;
using dot_net_app.Service.AccountService;
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

        // POST: api/account/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest createUserRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = new User
                {
                    Username = createUserRequest.Username,
                    Email = createUserRequest.Email,
                    PasswordHash = createUserRequest.PasswordHash,
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

                var userData = await _userService.CreateUserAsync(user);

                return Ok(userData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
