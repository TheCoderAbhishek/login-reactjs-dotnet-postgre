using dot_net_app.Model.AccountModel;
using System.ComponentModel.DataAnnotations;

namespace dot_net_app.Model.Shared
{
    public class VerifyOtpRequest
    {
        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "OTP is required")]
        public string? Otp { get; set; }

        public User? User { get; set; }
    }
}
