using System.ComponentModel.DataAnnotations;

namespace dot_net_app.Model.AccountModel
{
    public class CreateUserRequest
    {
        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        public string? PasswordHash { get; set; }
        public string? FullName { get; set; }
        public string? MobileNumber { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
