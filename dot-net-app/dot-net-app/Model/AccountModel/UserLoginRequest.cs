using System.ComponentModel.DataAnnotations;

namespace dot_net_app.Model.AccountModel
{
    public class UserLoginRequest
    {
        [Required(ErrorMessage = "Username or Email is required")]
        public string? UsernameOrEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? PasswordHash { get; set; }
    }
}
